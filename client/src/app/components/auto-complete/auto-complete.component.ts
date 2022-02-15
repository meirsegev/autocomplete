import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { fromEvent, of, ReplaySubject } from 'rxjs';
import { debounceTime, distinctUntilChanged, repeat, tap, map, switchMap, finalize, takeUntil, catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AutoCompleteService } from 'src/app/services/auto-complete.service';

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.css']
})
export class AutoCompleteComponent implements OnInit, AfterViewInit, OnDestroy {

  // ref to the searchInput element
  @ViewChild('searchInput') input?: ElementRef;

  private destroyed$: ReplaySubject<boolean> = new ReplaySubject(1);
  suggestions: string[] = [];
  isLoading: boolean = false;
  errorMsg: string = "";

  constructor(
    private http: HttpClient,
    private autoCompleteService: AutoCompleteService
  ) { }

  ngOnInit(): void {
    console.log('initialize...');
    this.autoCompleteService.initialize().subscribe(
      () => {
        console.log('auto complete service initialized')
      }, 
      (err) => {
        console.log(err);
    })
  }

  ngAfterViewInit() {
    const onKeyUpOb = fromEvent<any>(this.input?.nativeElement, 'keyup');
    onKeyUpOb.pipe(
        map(event => event.target.value),
        debounceTime(300),
        distinctUntilChanged(),
        tap((v) => {
          console.log("keyup event fired with:", v);
          this.errorMsg = "";
          this.isLoading = true;
        }),
        switchMap(prefix => this.autoCompleteService.getSuggestions(prefix as string)
          .pipe(
            finalize(() => {
              this.isLoading = false;
            })
          )
        ),
        takeUntil(this.destroyed$),
        catchError((err) => {
          console.log('error handled:', err);
          return of(null);
        }),
        repeat(), 
      )
      .subscribe(
        data => {
          console.log("data:", data);
          this.suggestions = data?? [];
        },
        err => {
          console.log("err:", err);
        }
      );
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }

  selected(ev: any) {
    (this.input?.nativeElement as HTMLInputElement).value = ev.innerText;
    this.suggestions = [];
  }
}

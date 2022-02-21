import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { fromEvent, Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, repeat, tap, map, switchMap, filter, finalize, catchError } from 'rxjs/operators';
import { AutoCompleteService } from 'src/app/services/auto-complete.service';

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.css']
})
export class AutoCompleteComponent implements OnInit, AfterViewInit {

  // ref to the searchInput element
  @ViewChild('searchInput') input?: ElementRef;
  isLoading: boolean = false;

  // this observable is getting un-subscribe automatically by 
  // angular framework due to the async pipe usage
  suggestionsOb$: Observable<string[]>;
  isShowSuggestions: boolean = true;

  constructor(
    private autoCompleteService: AutoCompleteService
  ) { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    // we are subscribing here(ngAfterViewInit) to the input element 
    // to be sure we have a reference to the input child element
    const onKeyUpOb = fromEvent<any>(this.input?.nativeElement, 'keyup');
    this.suggestionsOb$ = onKeyUpOb.pipe(
      debounceTime(200),
      filter(Boolean),
      map(event => ((event as any).target.value)),
      distinctUntilChanged(),
      tap((v) => {
        console.log("keyup event fired with:", v);
        this.isLoading = true;
      }),
      switchMap(prefix => this.autoCompleteService.getSuggestions(prefix as string)
        .pipe(
          finalize(() => {
            this.isLoading = false;
            this.isShowSuggestions = true;
          })
        )
      ),
      catchError((err) => {
        console.log('error handled:', err);
        return of(null);
      }),
      repeat(),
      map(response => response ?? [])
    );
  }

  selected(ev: any) {
    (this.input?.nativeElement as HTMLInputElement).value = ev.innerText;
    this.isShowSuggestions = false;
  }
}

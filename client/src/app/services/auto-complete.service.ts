import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AutoCompleteService {

  constructor(private http: HttpClient) {

   }

   getSuggestions(prefix: string): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiBase}/api/auto-complete/get-suggestions?prefix=${prefix.toLowerCase()}`);
   }
} 

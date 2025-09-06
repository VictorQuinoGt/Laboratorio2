import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE } from '../core/api.tokens';
import { Person } from '../models/person';

@Injectable({ providedIn: 'root' })
export class PersonService {
  private http = inject(HttpClient);
  private API = inject(API_BASE);
  private url = `${this.API}/api/Person`;

  getAll(): Observable<Person[]> { return this.http.get<Person[]>(this.url); }
  getById(id: number): Observable<Person> { return this.http.get<Person>(`${this.url}/${id}`); }
  add(p: Person): Observable<Person> { return this.http.post<Person>(this.url, p); }
  update(id: number, p: Person): Observable<Person> { return this.http.put<Person>(`${this.url}/${id}`, p); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE } from '../core/api.tokens';
import { Item } from '../models/item';

@Injectable({ providedIn: 'root' })
export class ItemService {
  private http = inject(HttpClient);
  private API = inject(API_BASE);
  private url = `${this.API}/api/Items`;

  getAll(): Observable<Item[]> { return this.http.get<Item[]>(this.url); }
  getById(id: number): Observable<Item> { return this.http.get<Item>(`${this.url}/${id}`); }
  add(i: Item): Observable<Item> { return this.http.post<Item>(this.url, i); }
  update(id: number, i: Item): Observable<Item> { return this.http.put<Item>(`${this.url}/${id}`, i); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
}

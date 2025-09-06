import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE } from '../core/api.tokens';
import { OrderCreateDto, OrderReadDto } from '../models/order';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);
  private API = inject(API_BASE);
  private url = `${this.API}/api/Orders`;

  create(payload: OrderCreateDto): Observable<OrderReadDto> {
    return this.http.post<OrderReadDto>(this.url, payload);
  }
  getById(id: number): Observable<OrderReadDto> {
    return this.http.get<OrderReadDto>(`${this.url}/${id}`);
  }

  getAll() {
  return this.http.get<OrderReadDto[]>(this.url);
 }
}

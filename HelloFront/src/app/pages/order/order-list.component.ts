import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { OrderService } from '../../services/order.service';
import { OrderReadDto } from '../../models/order';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './order-list.component.html'
})
export class OrderListComponent {
  private svc = inject(OrderService);
  private router = inject(Router);

  orders = signal<OrderReadDto[]>([]);
  displayed = ['number', 'person', 'createdAt', 'total', 'actions'];

  ngOnInit() { this.svc.getAll().subscribe(x => this.orders.set(x)); }
  view(o: OrderReadDto) { this.router.navigate(['/orders', o.id]); }
}

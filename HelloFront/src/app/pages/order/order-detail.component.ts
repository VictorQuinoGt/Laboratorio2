import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { OrderService } from '../../services/order.service';
import { OrderReadDto } from '../../models/order';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatTableModule],
  templateUrl: './order-detail.component.html'
})
export class OrderDetailComponent {
  private route = inject(ActivatedRoute);
  private svc = inject(OrderService);

  order = signal<OrderReadDto | null>(null);
  displayed = ['item', 'qty', 'price', 'total'];

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.svc.getById(id).subscribe(o => this.order.set(o));
  }
}

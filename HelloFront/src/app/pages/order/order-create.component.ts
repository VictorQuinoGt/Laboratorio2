import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

import { PersonService } from '../../services/person.service';
import { ItemService } from '../../services/item.service';
import { OrderService } from '../../services/order.service';
import { Person } from '../../models/person';
import { Item } from '../../models/item';
import { OrderCreateDto } from '../../models/order';


@Component({
  selector: 'app-order-create',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterModule,
    MatCardModule, MatButtonModule, MatFormFieldModule,
    MatSelectModule, MatInputModule, MatIconModule
  ],
  templateUrl: './order-create.component.html',
  styleUrls: ['./order-create.component.css']
})
export class OrderCreateComponent {
  private fb = inject(FormBuilder);
  private personSvc = inject(PersonService);
  private itemSvc = inject(ItemService);
  private orderSvc = inject(OrderService);

  persons = signal<Person[]>([]);
  items = signal<Item[]>([]);

  form = this.fb.group({
    personId: [null as number | null, Validators.required],
    lines: this.fb.array([] as any[])
  });

  get lines(): FormArray { return this.form.get('lines') as FormArray; }

  ngOnInit() {
    this.personSvc.getAll().subscribe(p => this.persons.set(p));
    this.itemSvc.getAll().subscribe(i => {
      this.items.set(i);
      if (this.lines.length === 0) this.addLine();
    });
  }

  addLine() {
    this.lines.push(this.fb.group({
      itemId: [null, Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      price: [null]
    }));
  }
  removeLine(i: number) { this.lines.removeAt(i); }

  lineTotal = (i: number) => {
    const g = this.lines.at(i);
    const itemId = g.get('itemId')?.value as number | null;
    const qty = +(g.get('quantity')?.value || 0);
    const override = g.get('price')?.value as number | null;
    const item = this.items().find(x => x.id === itemId);
    const price = override ?? item?.price ?? 0;
    return price * qty;
  };

  grandTotal = computed(() =>
    this.lines.controls.reduce((acc, _, i) => acc + this.lineTotal(i), 0)
  );

  submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const val = this.form.value;

    const payload: OrderCreateDto = {
      personId: val.personId!,
      lines: (val.lines || []).map((l: any) => ({
        itemId: l.itemId,
        quantity: +l.quantity,
        price: l.price ? +l.price : undefined
      }))
    };

    this.orderSvc.create(payload).subscribe({
      next: (res) => {
        alert(`Orden #${res.number} creada. Total: ${res.grandTotal}`);
        this.form.reset(); this.lines.clear(); this.addLine();
      },
      error: (e) => {
        console.error(e);
        alert('Error al crear la orden');
      }
    });
  }
}

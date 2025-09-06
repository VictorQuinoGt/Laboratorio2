import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ItemService } from '../../services/item.service';

@Component({
  selector: 'app-item-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, MatCardModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './item-form.component.html',
  styleUrls: ['./item-form.component.css']
})
export class ItemFormComponent {
  private fb = inject(FormBuilder);
  private svc = inject(ItemService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  id = signal<number | null>(null);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    price: [0, [Validators.required, Validators.min(0)]],
  });

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id.set(+idParam);
      this.svc.getById(+idParam).subscribe(it => this.form.patchValue(it));
    }
  }

  save() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const value = this.form.value;

    if (this.id()) {
      this.svc.update(this.id()!, value as any).subscribe(() => this.router.navigate(['/items']));
    } else {
      this.svc.add(value as any).subscribe(() => this.router.navigate(['/items']));
    }
  }
}

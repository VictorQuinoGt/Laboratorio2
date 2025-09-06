import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-person-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, MatCardModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.css']
})
export class PersonFormComponent {
  private fb = inject(FormBuilder);
  private svc = inject(PersonService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  id = signal<number | null>(null);

  form = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
  });

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id.set(+idParam);
      this.svc.getById(+idParam).subscribe(p => this.form.patchValue(p));
    }
  }

  save() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const value = this.form.value;

    if (this.id()) {
      this.svc.update(this.id()!, value as any).subscribe(() => this.router.navigate(['/persons']));
    } else {
      this.svc.add(value as any).subscribe(() => this.router.navigate(['/persons']));
    }
  }
}

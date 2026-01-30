import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface CarInsuranceQuote {
  regNumber: string;
  make: string;
  model: string;
  year: number;
  insurancePrice: number;
  coverage: string;
  bonus: number;
  deductible: number;
  coverageOptions: CoverageOption[];
}

interface CoverageOption {
  name: string;
  price: number;
  description: string;
}

interface ContactRequest {
  regNumber: string;
  phoneNumber: string;
  email: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  private http = inject(HttpClient);
  private apiUrl = '/api';

  regInput = signal('');
  result = signal<CarInsuranceQuote | null>(null);
  isLoading = signal(false);
  hasSearched = signal(false);
  error = signal<string | null>(null);

  // Contact form
  phoneNumber = signal('');
  email = signal('');
  isSubmittingContact = signal(false);
  contactSubmitted = signal(false);

  onSubmit(): void {
    const regNumber = this.regInput().replace(/\s/g, '').toUpperCase();
    if (!regNumber) return;

    this.isLoading.set(true);
    this.hasSearched.set(false);
    this.result.set(null);
    this.error.set(null);
    this.contactSubmitted.set(false);

    this.http.get<CarInsuranceQuote>(`${this.apiUrl}/bilforsikring/${regNumber}/tilbud`).subscribe({
      next: (quote) => {
        this.result.set(quote);
        this.isLoading.set(false);
        this.hasSearched.set(true);
      },
      error: (err) => {
        this.error.set('Kunne ikke hente pristilbud. Prøv igjen senere.');
        this.isLoading.set(false);
        this.hasSearched.set(true);
        console.error('Error fetching quote:', err);
      }
    });
  }

  onContactSubmit(): void {
    if (!this.phoneNumber() && !this.email()) return;
    if (!this.result()) return;

    this.isSubmittingContact.set(true);

    const request: ContactRequest = {
      regNumber: this.result()!.regNumber.replace(/\s/g, ''),
      phoneNumber: this.phoneNumber(),
      email: this.email()
    };

    this.http.post(`${this.apiUrl}/kontakt-meg`, request).subscribe({
      next: () => {
        this.isSubmittingContact.set(false);
        this.contactSubmitted.set(true);
      },
      error: (err) => {
        this.isSubmittingContact.set(false);
        console.error('Error submitting contact:', err);
      }
    });
  }

  formatPrice(price: number): string {
    return price.toLocaleString('nb-NO') + ' kr/mnd';
  }

  formatYearlyPrice(monthlyPrice: number): string {
    return (monthlyPrice * 12).toLocaleString('nb-NO') + ' kr/år';
  }

  get isContactFormValid(): boolean {
    return !!(this.phoneNumber() || this.email());
  }
}

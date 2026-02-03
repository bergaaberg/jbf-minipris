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

  // Offer details
  bonusOptions = [0, 10, 20, 30, 40, 50, 60, 70, 75];
  selectedBonus = signal(70);

  // Estimate form for when user can't find car
  showEstimateForm = signal(false);
  estimateMake = signal('');
  estimateModel = signal('');
  estimateYear = signal<number | null>(null); estimateMileage = signal<number | null>(8000); isEstimating = signal(false);

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
        this.selectedBonus.set(quote.bonus);
        this.isLoading.set(false);
        this.hasSearched.set(true);
      },
      error: (err: any) => {
        if (err?.status === 404) {
          this.error.set('Fant ingen bil med dette registreringsnummeret.');
        } else {
          this.error.set('Kunne ikke hente pristilbud. Prøv igjen senere.');
        }

        this.isLoading.set(false);
        this.hasSearched.set(true);
        console.error('Error fetching quote:', err);
      }
    });
  }

  toggleEstimateForm(): void {
    const next = !this.showEstimateForm();
    this.showEstimateForm.set(next);
    if (next) {
      this.hasSearched.set(false);
      this.result.set(null);
      this.error.set(null);
    }
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

  onEstimateSubmit(): void {
    if (!this.estimateMake() || !this.estimateModel() || !this.estimateYear() || !this.estimateMileage()) return;

    this.isEstimating.set(true);
    this.hasSearched.set(false);
    this.result.set(null);
    this.error.set(null);

    const payload = {
      make: this.estimateMake(),
      model: this.estimateModel(),
      year: Number(this.estimateYear()),
      mileage: Number(this.estimateMileage())
    } as any;

    this.http.post<CarInsuranceQuote>(`${this.apiUrl}/bilforsikring/estimat`, payload).subscribe({
      next: (quote) => {
        this.result.set(quote);
        this.selectedBonus.set(quote.bonus);
        this.isEstimating.set(false);
        this.hasSearched.set(true);
        this.showEstimateForm.set(false);
      },
      error: (err) => {
        this.error.set('Kunne ikke hente pristilbud. Prøv igjen senere.');
        this.isEstimating.set(false);
        this.hasSearched.set(true);
        console.error('Error fetching estimate:', err);
      }
    });
  }

  formatPrice(price: number): string {
    return price.toLocaleString('nb-NO') + ' kr/mnd';
  }

  formatYearlyPrice(monthlyPrice: number): string {
    const yearly = monthlyPrice * 12;
    return yearly.toLocaleString('nb-NO').replace(/\u00A0/g, ' ') + ' kr/år';
  }

  formatCurrency(amount: number): string {
    return amount.toLocaleString('nb-NO').replace(/\u00A0/g, ' ') + ' kr';
  }

  get isContactFormValid(): boolean {
    return !!(this.phoneNumber() && this.email());
  }
}

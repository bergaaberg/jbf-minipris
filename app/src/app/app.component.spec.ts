import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import {beforeEach, describe, expect, it } from "vitest";

describe('AppComponent', () => {
    let component: AppComponent;
    let fixture: ComponentFixture<AppComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [AppComponent],
            providers: [
                provideHttpClient(),
                provideHttpClientTesting()
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(AppComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should format monthly price correctly', () => {
        const formatted = component.formatPrice(500);
        expect(formatted).toBe('500 kr/mnd');
    });

    it('should format yearly price correctly', () => {
        const formatted = component.formatYearlyPrice(100);
        expect(formatted).toBe('1 200 kr/år');
    });

    it('should calculate yearly from monthly price', () => {
        const yearly = component.formatYearlyPrice(50);
        expect(yearly).toBe('500 kr/år');
    });

    it('should have correct number of bonus options', () => {
        expect(component.bonusOptions.length).toBe(8);
    });

    it('should include max bonus of 80%', () => {
        expect(component.bonusOptions).toContain(80);
    });
});

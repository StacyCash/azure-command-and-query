import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SubscriptionRequest } from '../models/subscriptionRequest';

@Injectable()
export class SubscriptionService {
  constructor(private readonly httpClient: HttpClient) {}

  async subscribe(subscriber: SubscriptionRequest): Promise<void> {
    const url = `https://localhost:44343/api/bookclubsignup`;

    await this.httpClient.post<number>(url, subscriber).toPromise();
  }
}

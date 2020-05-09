import { Component, OnInit } from '@angular/core';
import { SubscriptionService } from '../service/subscription.service';
import { SubscriptionRequest } from '../models/subscriptionRequest';

@Component({
  selector: 'app-intro',
  templateUrl: './subscribe.component.html',
  styleUrls: ['./subscribe.component.scss']
})
export class SubscribeComponent implements OnInit {
  feedbackList: Feedback[];
  model: SubscriptionRequest = {
    name: 'My Name',
    email: 'mail@mail.me',
    genre: 'All Genres'
  };
  constructor(private subscriptionService: SubscriptionService) { }

  ngOnInit() {
    this.feedbackList = [];
  }

  submitSubscription() {
    this.subscriptionService.subscribe(this.model);
    this.feedbackList.push({
      message: `${this.model.name} with ${this.model.email} signing up to genre  ${this.model.genre}  subscription sent`,
      time: new Date()}
    );

    this.trimFeedback();
  }

  private trimFeedback() {
    if (this.feedbackList.length > 5) {
      this.feedbackList = this.feedbackList.slice(1, this.feedbackList.length);
    }
  }
}

class Feedback{
  message: string;
  time: Date;
}

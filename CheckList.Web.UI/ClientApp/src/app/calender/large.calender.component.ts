import { Component, ViewChild, TemplateRef, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { startOfDay, endOfDay, subDays, addDays, endOfMonth, isSameDay, isSameMonth, addHours } from 'date-fns';
import { Subject } from 'rxjs';
import { CalendarEvent, CalendarEventAction, CalendarEventTimesChangedEvent, CalendarView } from 'angular-calendar';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpWrapper } from '../services/http.wrapper.service';
import { CalendarEventEditModel } from '../Model/calender.event.edit.model';
import { formatDate } from '@angular/common';

const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3'
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF'
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA'
  }
};

const url: string = 'api/event/';

@Component({
  selector: 'large-calender',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: ['large.calender.component.css'], encapsulation: ViewEncapsulation.None,
  templateUrl: './large.calender.component.html',
})

export class LargeCalenderComponent {
  @ViewChild('modalContent', { static: true } as any) modalContent: TemplateRef<any>;
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  viewDate: Date = new Date();

  public userEvents: CalendarEventEditModel[] = [];
  public newEvent: CalendarEvent;
  public eventEditModel: CalendarEventEditModel = new CalendarEventEditModel();

  modalData: {
    action: string;
    event: CalendarEvent;
  };


  constructor(private modal: NgbModal, private _httpWrapper: HttpWrapper) { }

  ngOnInit() {
    this.getEvents();
  }


  /*Code from the plug in */

  actions: CalendarEventAction[] = [
    {
      label: '<i class="fa fa-fw fa-pencil"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.handleEvent('Edited', event);
      }
    },
    {
      label: '<i class="fa fa-fw fa-times"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.events = this.events.filter(iEvent => iEvent !== event);
        this.handleEvent('Deleted', event);
      }
    }
  ];

  refresh: Subject<any> = new Subject();

  events: CalendarEvent[] = [
    {
      start: subDays(startOfDay(new Date()), 1),
      end: addDays(new Date(), 1),
      title: 'A 3 day event',
      color: colors.red,
      actions: this.actions,
      allDay: true,
      resizable: {
        beforeStart: true,
        afterEnd: true
      },
      draggable: true
    }
  ];

  activeDayIsOpen: boolean = true;

  setView(view: CalendarView) {
    this.view = view;
  }

  closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
      this.viewDate = date;
    }
  }

  eventTimesChanged({
    event,
    newStart,
    newEnd
  }: CalendarEventTimesChangedEvent): void {
    //this.events = this.events.map(iEvent => {
    //  if (iEvent === event) {
    //    return {
    //      ...event,
    //      start: newStart,
    //      end: newEnd
    //    };
    //  }
    //  return iEvent;
    //});
    this.handleEvent('Dropped or resized', event);
  }


  handleEvent(action: string, event: CalendarEvent): void {
    this.modalData = { event, action };
    this.modal.open(this.modalContent, { size: 'lg' });
  }

  /*Code from plug in ends */

  addEvent(): void {
    let that = this;
    var newEventEditModel = that.createModel();
    that._httpWrapper.post({ url: url + "createevent", data: newEventEditModel }).then(function (result) {
    });

  }

  getEvents() {
    let that = this;
    that._httpWrapper.get({ url: url + "getevent/1" }).then(function (result) {
      that.userEvents = result;
    });
  }

  deleteEvent(eventToDelete: CalendarEvent) {
    //APIS
    //this.events = this.events.filter(event => event !== eventToDelete);
  }


  createModel() {

    let newEvent: CalendarEventEditModel =
    {
      Id: 0,
      personId: 1,
      title: this.eventEditModel.title,
      start: this.eventEditModel.start,
      end: this.eventEditModel.end,
      draggable: true,
      allDay: false,
      resizableAfterEnd: false,
      resizableBeforeStart: true
    }

    return newEvent;
  }

}


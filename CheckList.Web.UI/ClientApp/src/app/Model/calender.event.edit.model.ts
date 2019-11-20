//import { CalendarEvent} from 'angular-calendar';

export class CalendarEventEditModel {
  Id: number = 0;
  personId: number = 0;
  start: Date = new Date();
  end: Date = new Date();
  title: string = '';
  allDay = false;
  draggable: boolean = true;
  resizableBeforeStart = true;
  resizableAfterEnd = false;
}

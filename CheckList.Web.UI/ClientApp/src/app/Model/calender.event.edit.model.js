"use strict";
//import { CalendarEvent} from 'angular-calendar';
Object.defineProperty(exports, "__esModule", { value: true });
var CalendarEventEditModel = /** @class */ (function () {
    function CalendarEventEditModel() {
        this.Id = 0;
        this.personId = 0;
        this.start = new Date();
        this.end = new Date();
        this.title = '';
        this.allDay = false;
        this.draggable = true;
        this.resizableBeforeStart = true;
        this.resizableAfterEnd = false;
    }
    return CalendarEventEditModel;
}());
exports.CalendarEventEditModel = CalendarEventEditModel;
//# sourceMappingURL=calender.event.edit.model.js.map
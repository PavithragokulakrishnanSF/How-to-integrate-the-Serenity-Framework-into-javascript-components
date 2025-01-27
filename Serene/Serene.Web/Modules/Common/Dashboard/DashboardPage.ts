import { Schedule, Day, Week, WorkWeek, Month, Agenda } from '@syncfusion/ej2-schedule';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';

Schedule.Inject(Day, Week, WorkWeek, Month, Agenda);
const baseUrl = window.location.origin;

let dataManager: DataManager = new DataManager({
    url: `${baseUrl}/api/schedule/loaddata`, // 'controller/actions'
    crudUrl: `${baseUrl}/api/schedule/UpdateData`,
    adaptor: new UrlAdaptor(),
    crossDomain: true
});


let scheduleObj: Schedule = new Schedule({
    height: '550px',
    selectedDate: new Date(2025, 0, 27),
    eventSettings: { dataSource: dataManager }
});
scheduleObj.appendTo('#Schedule');
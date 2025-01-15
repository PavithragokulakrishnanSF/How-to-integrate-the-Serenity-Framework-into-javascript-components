import { Schedule, Week, WorkWeek, Month } from '@syncfusion/ej2-schedule';
let defaultData: Object[] = [
    {
        Id: 1,
        Subject: 'Conference',
        StartTime: new Date(2018, 1, 7, 10, 0),
        EndTime: new Date(2018, 1, 7, 11, 0),
        IsAllDay: false
    }, {
        Id: 2,
        Subject: 'Meeting - 1',
        StartTime: new Date(2018, 1, 15, 10, 0),
        EndTime: new Date(2018, 1, 16, 12, 30),
        IsAllDay: false
    }, {
        Id: 3,
        Subject: 'Paris',
        StartTime: new Date(2018, 1, 13, 12, 0),
        EndTime: new Date(2018, 1, 13, 12, 30),
        IsAllDay: false
    }, {
        Id: 4,
        Subject: 'Vacation',
        StartTime: new Date(2018, 1, 12, 10, 0),
        EndTime: new Date(2018, 1, 12, 10, 30),
        IsAllDay: false
    }
];

Schedule.Inject( Week, WorkWeek, Month );

let scheduleObj: Schedule = new Schedule({
    width: '100%',
    height: '550px',
    selectedDate: new Date(2018, 1, 15),
    views: [{ option: 'Week', startHour: '07:00', endHour: '15:00'},
            { option: 'WorkWeek', startHour: '10:00', endHour: '18:00'},
            { option: 'Month' , showWeekend: false }],
    eventSettings: { dataSource: defaultData }
});
scheduleObj.appendTo('#Schedule');
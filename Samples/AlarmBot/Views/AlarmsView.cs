using AlarmBot.Models;
using Microsoft.Bot.Builder;
using System.Collections.Generic;

namespace AlarmBot.Views
{
    public static class AlarmsView
    {
        public static void ShowAlarms(ITurnContext turnContext, List<Alarm> alarms)
        {
            if ((alarms == null) || (alarms.Count == 0))
            {
                turnContext.SendActivity("You have no alarms.");
                return;
            }

            if (alarms.Count == 1)
            {
                turnContext.SendActivity($"You have one alarm named '{ alarms[0].Title }', set for '{ alarms[0].Time }'.");
                return;
            }

            string message = $"You have { alarms.Count } alarms: \n\n";
            foreach (var alarm in alarms)
            {
                message += $"'{ alarm.Title }' set for '{ alarm.Time }' \n\n";
            }

            turnContext.SendActivity(message);
        }
    }
}

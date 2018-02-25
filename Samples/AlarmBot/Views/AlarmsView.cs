using AlarmBot.Models;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmBot.Views
{
    public static class AlarmsView
    {
        public static void ShowAlarms(IBotContext context, List<Alarm> alarms)
        {
            if ((alarms == null) || (alarms.Count == 0))
            {
                context.Reply("You have no alarms.");
                return;
            }

            if (alarms.Count == 1)
            {
                context.Reply($"You have one alarm named '{ alarms[0].Title }', set for '{ alarms[0].Time }'.");
                return;
            }

            string message = $"You have { alarms.Count } alarms: \n\n";
            foreach (var alarm in alarms)
            {
                message += $"'{ alarm.Title }' set for '{ alarm.Time }' \n\n";
            }

            context.Reply(message);
        }
    }
}

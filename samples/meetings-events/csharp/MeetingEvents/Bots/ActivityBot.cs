// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace ReceiveMessagesWithRSC.Bots
{
    using AdaptiveCards;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class ActivityBot : TeamsActivityHandler
    {
        /// <summary>
        /// Activity Handler for Meeting start event
        /// </summary>
        /// <param name="meeting"></param>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task OnTeamsMeetingStartAsync(MeetingStartEventDetails meeting, ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            MeetingInfo meetingInfo = await TeamsInfo.GetMeetingInfoAsync(turnContext);

            await turnContext.SendActivityAsync(MessageFactory.Attachment(GetAdaptiveCardForMeetingStart(meeting, meetingInfo)));
        }

        /// <summary>
        /// Activity Handler for Meeting end event
        /// </summary>
        /// <param name="meeting"></param>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task OnTeamsMeetingEndAsync(MeetingEndEventDetails meeting, ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(MessageFactory.Attachment(GetAdaptiveCardForMeetingEnd(meeting)));
        }

        /// <summary>
        /// Sample Adaptive card for Meeting Start event.
        /// </summary>
        private Attachment GetAdaptiveCardForMeetingStart(MeetingStartEventDetails meeting, MeetingInfo meetingInfo)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion("1.2"))
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = meeting.Title  + "- started",
                        Weight = AdaptiveTextWeight.Bolder,
                        Spacing = AdaptiveSpacing.Medium,
                    },
                                         new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = "Start Time : ",
                                        Wrap = true,
                                    },
                                },
                            },
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = Convert.ToString(meeting.StartTime),
                                        Wrap = true,
                                    },
                                },
                            },
                        },
                    },
                },
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveOpenUrlAction
                    {
                        Title = "Join meeting",
                        Url = meeting.JoinUrl,
                    },
                },
            };

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };
        }

        /// <summary>
        /// Sample Adaptive card for Meeting End event.
        /// </summary>
        private Attachment GetAdaptiveCardForMeetingEnd(MeetingEndEventDetails meeting)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion("1.2"))
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = meeting.Title  + "- ended",
                        Weight = AdaptiveTextWeight.Bolder,
                        Spacing = AdaptiveSpacing.Medium,
                    },
                     new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = "End Time : ",
                                        Wrap = true,
                                    },
                                },
                            },
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = Convert.ToString(meeting.EndTime),
                                        Wrap = true,
                                    },
                                },
                            },
                        },
                    },
                }
            };

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };
        }
    }
}
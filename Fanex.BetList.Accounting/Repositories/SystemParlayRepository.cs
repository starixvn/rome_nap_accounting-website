namespace Fanex.BetList.Accounting.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common;
    using Data;
    using Models;
    using NAP.Framework.Data;

    public class SystemParlayRepository : ISystemParlayRepository
    {
        public SystemParlayDetail CombinationDetails(ICriteria criteria)
        {
            var model = new SystemParlayDetail();
            model.SubTotal = 0;
            var serialHash = new Dictionary<string, Event>(1024 * 5);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    // Ticket result.
                    if (!reader.NextResult())
                    {
                        return model;
                    }

                    // Choice result.
                    if (!reader.NextResult())
                    {
                        return model;
                    }

                    string serial;
                    Event eventObject;

                    // Event result.
                    while (reader.Read())
                    {
                        serial = Converter.ToString2(reader["serial"]);

                        if (!serialHash.ContainsKey(serial))
                        {
                            eventObject = ParseEvent(reader);
                            serialHash.Add(serial, eventObject);

                            model.SubTotal += eventObject.WinLoss;
                        }
                    }

                    // Match result.
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            serial = Converter.ToString2(reader["serial"]);

                            if (serialHash.TryGetValue(serial, out eventObject))
                            {
                                eventObject.MatchList.Add(ParseMatch(reader));
                            }
                        }
                    }
                }
            }

            model.EventList = serialHash.Values.ToList();

            return model;
        }

        public SystemParlayDetailExcel GetSystemParlayDetailExcel(ICriteria criteria)
        {
            var model = new SystemParlayDetailExcel();
            model.Total = 0;

            var serialHash = new Dictionary<string, EventExcel>(1024 * 5);
            var loseEvents = new List<EventExcel>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    string serial;
                    EventExcel eventExcel;

                    // Event result.
                    while (reader.Read())
                    {
                        serial = Converter.ToString2(reader["serial"]);
                        eventExcel = ParseEventExcel(reader);

                        if (serial == "0")
                        {
                            loseEvents.Add(eventExcel);
                        }
                        else if (!serialHash.ContainsKey(serial))
                        {
                            serialHash.Add(serial, eventExcel);
                        }

                        model.Total += eventExcel.WinLoss + eventExcel.PlayerComm;
                    }

                    // Match result.
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            serial = Converter.ToString2(reader["serial"]);

                            if (serialHash.TryGetValue(serial, out eventExcel))
                            {
                                eventExcel.Matches.Add(ParseMatch(reader));
                            }
                        }
                    }

                    var outputParams = db.GetOuputParams();
                    model.Outstanding = Converter.ToDecimal(outputParams["outstanding"]);
                }
            }

            loseEvents.AddRange(serialHash.Values);
            model.Events = loseEvents;

            return model;
        }

        private Event ParseEvent(IDataReader reader)
        {
            return new Event
            {
                WinLossDate = Converter.ToDateTime(reader["winlostdate"]),
                Odds = Converter.ToDecimal(reader["modds"]),
                Stake = Converter.ToDecimal(reader["stake"]),
                WinLoss = Converter.ToDecimal(reader["winlost"]),
                Status = Converter.ToString2(reader["status"])
            };
        }

        private EventExcel ParseEventExcel(IDataReader reader)
        {
            return new EventExcel
            {
                WinLossDate = Converter.ToDateTime(reader["winlostdate"]).ToShortDateString(),
                Odds = Converter.ToDecimal(reader["modds"]),
                Stake = Converter.ToDecimal(reader["stake"]),
                WinLoss = Converter.ToDecimal(reader["winlost"]),
                Status = Converter.ToString2(reader["status"]),
                PlayerComm = Converter.ToDecimal(reader["playercomm"]),
            };
        }

        private Match ParseMatch(IDataReader reader)
        {
            return new Match
            {
                HomeID = Converter.ToInt(reader["homeid"]),
                AwayID = Converter.ToInt(reader["awayid"])
            };
        }
    }
}

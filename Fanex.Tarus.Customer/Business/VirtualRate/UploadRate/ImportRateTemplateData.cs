// --------------------------------------------------------------------------
// <copyright file="ImportRateTemplateData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.VirtualRate.UploadRate
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;
    using Fanex.Tarus.Infrastructure.Constants;

    /// <summary>
    /// Import Rate Template Data.
    /// </summary>
    public static class ImportRateTemplateData
    {
        /// <summary>
        /// Get this excel template.
        /// </summary>
        /// <returns>Excel Template.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static List<ImportVirtualRateResultGroup> Template()
        {
            List<ImportVirtualRateResultGroup> result = new List<ImportVirtualRateResultGroup>();
            result.Add(new ImportVirtualRateResultGroup
            {
                GroupName = "SuperTest1",
                ImportVirtualRateResultList = new List<ImportVirtualRateResult>()
                {
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Live",
                        BetTypeId = BetType.Handicap,
                        VirtualRateString = "-60"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Live",
                        BetTypeId = BetType.OverUnder,
                        VirtualRateString = "20"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Live",
                        BetTypeId = BetType.OddEven,
                        VirtualRateString = "50"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Live",
                        BetTypeId = BetType.FirstHandicap,
                        VirtualRateString = "40"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Live",
                        BetTypeId = BetType.FirstOverUnder,
                        VirtualRateString = "40"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Live",
                        BetTypeId = BetType.FirstOddEven,
                        VirtualRateString = "40"
                    },
                    null,
                    null,
                    null,
                    null
                }
            });

            result.Add(new ImportVirtualRateResultGroup
            {
                GroupName = "SuperTest1",
                ImportVirtualRateResultList = new List<ImportVirtualRateResult>()
                {
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.Handicap,
                        VirtualRateString = "20"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.OverUnder,
                        VirtualRateString = "50"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.OddEven,
                        VirtualRateString = "40"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.FirstHandicap,
                        VirtualRateString = "40"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.FirstOverUnder,
                        VirtualRateString = "-30"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.FirstOddEven,
                        VirtualRateString = "-30"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.MoneyLine,
                        VirtualRateString = "60"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.FirstMoneyLine,
                        VirtualRateString = "30"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.SetXWinner,
                        VirtualRateString = "40"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.SetXGameYWinner,
                        VirtualRateString = "30"
                    }
                }
            });

            result.Add(new ImportVirtualRateResultGroup
            {
                GroupName = "MasterTest1",
                ImportVirtualRateResultList = new List<ImportVirtualRateResult>()
                {
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.Handicap
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.OverUnder,
                        VirtualRateString = "20"
                    },
                    null,
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.FirstHandicap,
                        VirtualRateString = "20"
                    },
                    null,
                    new ImportVirtualRateResult
                    {
                        SportName = "Soccer",
                        LiveTypeName = "Non-Live",
                        BetTypeId = BetType.FirstOddEven,
                        VirtualRateString = "20"
                    },
                    null,
                    null,
                    null,
                    null
                }
            });

            result.Add(new ImportVirtualRateResultGroup
            {
                GroupName = "AgentTest1",
                ImportVirtualRateResultList = new List<ImportVirtualRateResult>()
                {
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = string.Empty,
                        BetTypeId = BetType.Handicap
                    },
                    null,
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = string.Empty,
                        BetTypeId = BetType.OddEven,
                        VirtualRateString = "15"
                    },
                    null,
                    null,
                    null,
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = string.Empty,
                        BetTypeId = BetType.MoneyLine,
                        VirtualRateString = "60"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = string.Empty,
                        BetTypeId = BetType.FirstMoneyLine,
                        VirtualRateString = "60"
                    },
                    new ImportVirtualRateResult
                    {
                        SportName = "Other Sports",
                        LiveTypeName = string.Empty,
                        BetTypeId = BetType.SetXWinner,
                        VirtualRateString = "30"
                    },
                    null
                }
            });

            return result;
        }
    }
}

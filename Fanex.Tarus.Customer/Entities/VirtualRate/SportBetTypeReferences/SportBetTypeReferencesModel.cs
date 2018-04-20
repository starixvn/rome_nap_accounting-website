namespace Fanex.Tarus.Customer.Entities.VirtualRate.SportBetTypeReferences
{
    using System.Collections.Generic;

    public class SportBetTypeReferencesModel : BaseModel
    {
        public List<SourceReferenceModel> SourceReferences { get; set; }

        public List<SportBetTypeReferencesModel> SubTypes { get; set; }
    }
}

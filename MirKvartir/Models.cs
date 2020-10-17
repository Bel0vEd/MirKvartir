using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirKvartir
{
    class Models
    {
        public class RentPeriod
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class AllEstateType
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class Material
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class Renovation
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class ToiletType
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class Parking
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class ContractTypeViewModel
        {
            public string text { get; set; }
            public string value { get; set; }
            public List<string> allowedListingTypes { get; set; }
            public List<string> allowedEstateTypes { get; set; }
        }

        public class SubwayDistanceType
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class PrepaymentDayss
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class PledgeDayss
        {
            public string text { get; set; }
            public int? value { get; set; }
        }

        public class ContractType
        {
            public string text { get; set; }
        }

        public class LocationId
        {
            public int LocalId { get; set; }
            public int MetaType { get; set; }
            public string Category { get; set; }
        }

        public class SuggestItem
        {
            public LocationId LocationId { get; set; }
            public int LocationType { get; set; }
            public string LocationFullName { get; set; }
            public string LocationFullNameReversed { get; set; }
            public bool IsLeaf { get; set; }
            public int Weight { get; set; }
        }

        public class Location2
        {
            public int LocalId { get; set; }
            public int MetaType { get; set; }
            public string Category { get; set; }
        }

        public class Value
        {
            public SuggestItem SuggestItem { get; set; }
            public bool IsLeaf { get; set; }
            public List<Location2> Locations { get; set; }
        }

        public class Location
        {
            public string label { get; set; }
            public string cleanLabel { get; set; }
            public Value value { get; set; }
        }

        public class LocationId2
        {
            public int LocalId { get; set; }
            public int MetaType { get; set; }
            public string Category { get; set; }
        }

        public class SuggestItem2
        {
            public LocationId2 LocationId { get; set; }
            public int LocationType { get; set; }
            public string LocationFullName { get; set; }
            public string LocationFullNameReversed { get; set; }
            public bool IsLeaf { get; set; }
            public int Weight { get; set; }
        }

        public class Location3
        {
            public int LocalId { get; set; }
            public int MetaType { get; set; }
            public string Category { get; set; }
        }

        public class Value2
        {
            public SuggestItem2 SuggestItem { get; set; }
            public bool IsLeaf { get; set; }
            public List<Location3> Locations { get; set; }
        }

        public class LocationAuto
        {
            public string label { get; set; }
            public string cleanLabel { get; set; }
            public Value2 value { get; set; }
        }

        public class SelectedLocation
        {
            public string label { get; set; }
            public List<string> value { get; set; }
        }

        public class EstateType
        {
            public bool disabled { get; set; }
            public object group { get; set; }
            public bool selected { get; set; }
            public string text { get; set; }
            public string value { get; set; }
        }

        public class RootObject
        {
            public string estateId { get; set; }
            public int mainId { get; set; }
            public string created { get; set; }
            public int updated { get; set; }
            public List<RentPeriod> rentPeriods { get; set; }
            public List<AllEstateType> allEstateTypes { get; set; }
            public List<Material> materials { get; set; }
            public List<Renovation> renovations { get; set; }
            public List<ToiletType> toiletTypes { get; set; }
            public List<Parking> parkings { get; set; }
            public List<int> regionsForLead { get; set; }
            public List<int> regionsForConsultation { get; set; }
            public List<int> townsForLead { get; set; }
            public List<int> regionsForGoodPhotosOffer { get; set; }
            public string listingType { get; set; }
            public string estateType { get; set; }
            public string rentPeriod { get; set; }
            public string price { get; set; }
            public int lat { get; set; }
            public int lng { get; set; }
            public object locationFromUrl { get; set; }
            public List<ContractTypeViewModel> contractTypeViewModels { get; set; }
            public object subway { get; set; }
            public object highway { get; set; }
            public object subwayTime { get; set; }
            public string subwayDistanceType { get; set; }
            public List<SubwayDistanceType> subwayDistanceTypes { get; set; }
            public object toMkad { get; set; }
            public int prepaymentDays { get; set; }
            public object pledgeDays { get; set; }
            public object clientFee { get; set; }
            public object haggle { get; set; }
            public object utilitiesIncludedInPrice { get; set; }
            public string description { get; set; }
            public object floors { get; set; }
            public object material { get; set; }
            public object roomCount { get; set; }
            public bool studio { get; set; }
            public bool openPlan { get; set; }
            public bool penthouse { get; set; }
            public bool apartment { get; set; }
            public object area { get; set; }
            public object areaRoom { get; set; }
            public object buildingYear { get; set; }
            public object balcony { get; set; }
            public object toiletType { get; set; }
            public object renovation { get; set; }
            public object parking { get; set; }
            public object conditioner { get; set; }
            public object modernWindows { get; set; }
            public object waterHeater { get; set; }
            public object stationaryPhone { get; set; }
            public object internet { get; set; }
            public object roomFurniture { get; set; }
            public object kitchenFurniture { get; set; }
            public object television { get; set; }
            public object washingMachine { get; set; }
            public object dishWasher { get; set; }
            public object refrigerator { get; set; }
            public object withPets { get; set; }
            public object withChildren { get; set; }
            public object lift { get; set; }
            public object freightLift { get; set; }
            public object floor { get; set; }
            public object buildingSeries { get; set; }
            public object areaLive { get; set; }
            public object areaKitchen { get; set; }
            public string areaRooms { get; set; }
            public object roomForSaleCount { get; set; }
            public object lotSize { get; set; }
            public bool gasSupply { get; set; }
            public bool sewerageSupply { get; set; }
            public bool alarm { get; set; }
            public bool hasPhone { get; set; }
            public bool heatingSupply { get; set; }
            public bool waterSupply { get; set; }
            public bool electricitySupply { get; set; }
            public bool garage { get; set; }
            public List<object> photos { get; set; }
            public string name { get; set; }
            public object phone { get; set; }
            public int accountType { get; set; }
            public List<object> leadRuHelpTypes { get; set; }
            public int highlight { get; set; }
            public string locationFromUrlName { get; set; }
            public string header { get; set; }
            public List<string> exportFields { get; set; }
            public List<PrepaymentDayss> prepaymentDayss { get; set; }
            public List<PledgeDayss> pledgeDayss { get; set; }
            public List<ContractType> contractTypes { get; set; }
            public bool estateTypeDropdownVisible { get; set; }
            public string estateTypeName { get; set; }
            public bool rentPeriodDropdownVisible { get; set; }
            public string rentPeriodName { get; set; }
            public bool subwayDistanceTypeDropdownVisible { get; set; }
            public string subwayDistanceTypeName { get; set; }
            public bool prepaymentDaysDropdownVisible { get; set; }
            public string prepaymentDaysName { get; set; }
            public bool pledgeDaysDropdownVisible { get; set; }
            public string pledgeDaysName { get; set; }
            public bool contractTypeDropdownVisible { get; set; }
            public string contractTypeName { get; set; }
            public Location location { get; set; }
            public LocationAuto locationAuto { get; set; }
            public SelectedLocation selectedLocation { get; set; }
            public int house { get; set; }
            public int street { get; set; }
            public int town { get; set; }
            public int region { get; set; }
            public bool flatEstateType { get; set; }
            public bool houseEstateType { get; set; }
            public bool landEstateType { get; set; }
            public bool moscowOrSpb { get; set; }
            public bool moscowOrSpbRegion { get; set; }
            public bool countryEstateType { get; set; }
            public bool showHighway { get; set; }
            public bool showToMkad { get; set; }
            public string subwayImage { get; set; }
            public List<EstateType> estateTypes { get; set; }
            public bool placemarkMovedByUser { get; set; }
            public bool isLeafLocation { get; set; }
        }
    }
}
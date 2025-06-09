namespace LiveMapDashboard.Web.Exceptions;

public class MapNotFoundException(string mapId) 
    : Exception($"No map was found for the given mapid of {mapId}");

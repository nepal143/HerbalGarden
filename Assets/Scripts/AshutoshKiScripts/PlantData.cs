using System.Collections.Generic;

[System.Serializable]
public class Plant
{
    public string name;
    public string scientificName;
    public string uses;
    public string benefits;
}

[System.Serializable]
public class PlantList
{
    public List<Plant> plants;
}

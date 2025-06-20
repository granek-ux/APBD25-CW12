﻿namespace APBD25_CW12.DTO;

public class TripsDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public ICollection<CountryDto> Countries { get; set; }
    public ICollection<ClientDto> Clients { get; set; }
}
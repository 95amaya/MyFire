namespace MyFireConsoleApp.Models;

public class Student
{
    public string FirstName {get; set;}
    public string Sex {get; set;}
    public string Class {get; set;}
    public string City {get; set;}
    public string Major {get; set;}

    public override string ToString()
    {
        return $"{FirstName}, {Major}, {City}";
    }
}
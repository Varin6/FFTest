
# FourteenFish Test - Kamil Kuklinski


## Setup:

1. If creating DB from scratch - run seed.sql where the below queries are already included, or if you already have the DB, run those queries to create tables needed for Specialisations:

```
create table specialities (
    Id INT PRIMARY KEY auto_increment,
    Name VARCHAR(100)
);

insert into specialities (Name) values ('Anaesthetics');
insert into specialities (Name) values ('Cardiology');
insert into specialities (Name) values ('Dermatology');
insert into specialities (Name) values ('Emergency Medicine');
insert into specialities (Name) values ('General Practice (GP)');
insert into specialities (Name) values ('Neurology');
insert into specialities (Name) values ('Obstetrics and Gynaecology');
insert into specialities (Name) values ('Ophthalmology');
insert into specialities (Name) values ('Orthopaedic Surgery');
insert into specialities (Name) values ('Psychiatry');

create table person_specialities (
    Id INT PRIMARY KEY auto_increment,
    SpecialityId INT,
    PersonId INT
);
```

2. Change/create .env file connection string to point to the DB you're using.

3. Make sure you're on `develop` branch



## Comments on the steps I have taken to solve those problems.

## Task 1

1. First, I added the DotNetEnv package to load the .env file containing the connection string to the MySQL database. I created and populated the database by running the provided seed.sql scripts.

2. I added a Services project to maintain separation of concerns. This project references the Models and DAL projects. I created the JsonImportService to handle the import logic.

3. Encountered issue with my import logic, some records don't have an address and this causes an exception where it should just skip the address in those cases. Fixing

4. The dilemma I faced was deciding what to demonstrate. Initially, I needed to deserialize JSON and map it to objects. I created small DTOs within the service class for this purpose. These DTOs needed to be mapped to actual entities and saved, but the Person entity had to be saved first to obtain its ID for the Address. Initially, I considered creating simple Add() methods in the repositories to iterate and save each entity one by one, checking if each already existed. However, this approach would create numerous database transactions, making it slow and unsuitable for large datasets. I started this approach and left some of the single-addition methods in the repositories, although they are now unused

The better approach was to use a transaction, which reduces database calls and saves all data at once. This is the method I chose, handling everything within one transaction and rolling back if something goes wrong. I first check whether the items already exist and save only those that don't.

For larger datasets, a better approach would be to use batches, creating one transaction per 100 or 1,000 items to avoid issues with very large queries. I decided not to implement this but to note it here.

5. Added success/error messages to the import view, as well as showing the count of the imported items if any have been imported.

6. Created BaseRepository and interface to make BeginTransactionAsync() available in each repository.

## Task 2

1. Added some sql to seed.sql file to add tables needed for Specialisation and specialisation associations. I ran those queries to create the needed tables.

2. Added SpecialitiesRepository, Controller Models, Views - by copying style and logic from Person related entities. First I replicated Index and Details/Edit pages and functions. I also added all the repository methods that I will probably need to complete this task.

3. Added Create functionality and Delete functionality as well as messages to show whether the actions ran succesfully. Delete Specialisation first removes all associations to Persons and then removes Speciality itself. 

4. I added the ability to add Specialisations to a Person in the Details (Edit) view. Instead of using the add/remove methods in the repository that I initially implemented, I created a method that takes a list of Specialisation IDs, clears the Person's old associations, and creates new ones from the received IDs. This approach uses a single transaction instead of comparing and adding/removing associations individually.

I used multiselect box for it, I don't personally like multiselext boxes very much, it could instead be a loop that outputs checkboxes or a more elaborate solution with two interactive columns where user can click on items and move them between the columns. For the purpose of the exercise, I think it's ok.
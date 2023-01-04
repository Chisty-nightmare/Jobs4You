create table Clients (

clientID int identity(10000,1) primary key,
client_username varchar(50) NOT NULL,
client_pass varchar(50) NOT NULL,
client_phone varchar(50),
client_stat int,
client_rating float,
client_mail varchar(50) unique,
client_name varchar(50) NOT NULL,
 
)

create table jobs(

jobID int identity(20000,1) primary key,
clientID int,
pricing float not null,
details varchar(500) not null,
duration varchar(50) not null,
foreign key (clientID) references Clients,
)

select * from Freelancers

Create table Freelancers(

freelancerID int identity(40000,1) primary key,
freelancer_username varchar(50) not null,
freelancer_pass varchar(50) not null,
freelancer_phone varchar(50),
freelancer_stat int,
freelancer_rating float,
freelancer_mail varchar(50) unique,
freelancer_name varchar(50) NOT NULL,
)



create table Hires(

HireID int identity(70000,1) primary key,
clientID int,
freelancerID int,
foreign key (clientID) references Clients on delete cascade,
foreign key (freelancerID) references Freelancers on delete cascade,

)



create table Payment(

paymentID int identity (110000,1) primary key,
clientID int,
jobID int,
clientPayStat int,
freelancerID int,
amount float not null,
frlncrReceiveStat int,
foreign key (clientID) references Clients on delete cascade,
foreign key (jobID) references jobs on delete cascade,
foreign key (freelancerID) references Freelancers on delete cascade,
) 

create table Invoice(

invoiceID int identity (140000,1) primary key,
paymentID int,
pricing float not null,
details varchar(500) not null,
foreign key (paymentID) references Payment on delete cascade,
)


create table ApplyJobs(
applyToken int identity(200000,1)  primary key,
jobID int not null,
freelancerID int not null,
foreign key (jobID) references jobs on delete cascade,
foreign key (freelancerID) references Freelancers on delete cascade,
)

select * from jobs
select * from Freelancers
select * from ApplyJobs

create table Skills(
skillID int identity (240000,1) primary key,
freelancerID int not null,
skills text,
foreign key (freelancerID) references Freelancers on delete cascade,
)

create table Admin(
adminID int identity(300000,1) primary key,
admin_pass varchar(50) not null,
admin_username varchar(50) not null,
admin_mail varchar(50) unique,
)

create table Rating(
ratingID int identity(610000,1) primary key,
paymentID int,
client_rating float,
freelancer_rating float,
foreign key (paymentID) references Payment on delete cascade,

)

delete from Skills
select * from Skills
select * from Freelancers
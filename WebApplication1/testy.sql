SELECT IDSTUDY FROM STUDIES WHERE NAME = 'Informatyka'

SELECT IDENROLLMENT FROM ENROLLMENT E, STUDIES S WHERE E.IDSTUDY = S.IDSTUDY AND S.NAME = 'Informatyka' AND SEMESTER = (6 + 1)

select * from enrollment where StartDate = (select max(StartDate) from enrollment)

select * from student

select * from studies

select * from enrollment

insert into enrollment (IdEnrollment, Semester, IdStudy, StartDate) values (40, 1, (select idstudy from studies where name = @name), GETDATE())

Select indexnumber from student where indexnumber = 's1234'

EXEC PromoteStudents 'Informatyka', 7

select idenrollment from enrollment where startdate = (select max(startdate) from enrollment)

select semester from enrollment e, studies s where e.idstudy = s.idstudy and e.Semester = '6' and s.name = 'Informatyka'

update student set idenrollment = 20;

delete from dbo.enrollment where idenrollment = 30;

delete from dbo.enrollment where idenrollment = 40;

delete from dbo.student where indexnumber = 's1234'

Insert into student (firstname, lastname, birthdate, indexnumber, idenrollment) values ('Andrzej', 'Malewski', '1993-03-30', 's1234', (select idenrollment from enrollment where startdate = (Select max(Startdate) from enrollment)) )

select idenrollment from enrollment where startdate = (select max(startdate) from enrollment)

Select indexnumber from student where indexnumber = 's7973'
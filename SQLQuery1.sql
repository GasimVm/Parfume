select *   from  [PARFUME_ADMIN].[Orders] o
where    o.IsCredite='false' and o.FirstPrice>0
 select *    from [PARFUME_ADMIN].[PaymentHistories] p
where p.Status=0 and p.PayPrice is null

select* from  [PARFUME_ADMIN].[Orders] o
join [PARFUME_ADMIN].[PaymentHistories] p
on o.Id=p.OrderId
where o.Debt>0 and p.Debt=0 and p.Status =1

select * from [PARFUME_ADMIN].[Customers] as c
where c.Fincode=N'1GUEVQS'
select *   from  [PARFUME_ADMIN].[Orders] o
where o.FirstPrice=137

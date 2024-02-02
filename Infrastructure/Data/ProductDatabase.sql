CREATE TABLE Categories (
	Id int not null identity primary key,
	CategoryName nvarchar(50) not null unique
)

CREATE TABLE Manufactures (
	Id int not null identity primary key,
	ManufactureName nvarchar(50) not null unique
)

CREATE TABLE Products (
	ArticleNumber nvarchar(450) not null primary key,
	CategoryId int not null foreign key (CategoryId) references Categories(Id),
	ManufactureId int not null foreign key (ManufactureId) references Manufactures(Id)
)

CREATE TABLE ProductInformation (
	ArticleNumber nvarchar(450) not null primary key foreign key (ArticleNumber) references Products(ArticleNumber),
	ProductTitle nvarchar(200) not null,
	Ingress nvarchar(200) null,
	Description nvarchar(max) null,
	Specification nvarchar(max) null,
)

CREATE TABLE ProductPrices (
	ArticleNumber nvarchar(450) not null primary key foreign key (ArticleNumber) references Products(ArticleNumber),
	Price money not null
)
Create DataBase CLIENTE_CRUD
Go

Use CLIENTE_CRUD
Go

Create table CLIENTE(
	[CLIE_PK_ID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[CLIE_NOME] [nvarchar](100) NOT NULL,
	[CLIE_DATA_NASCIMENTO] [datetime] null,
	[CLIE_TELEFONE] [nvarchar](20) null,
	[CLIE_IMAGEM] [varbinary](max) NULL,
	[CLIE_TIPO] [nvarchar](2) Null,
	[CLIE_EMAIL] [nvarchar](100) NULL,
	[CLIE_CPF] [nvarchar](14) NULL,
	[CLIE_CNPJ] [nvarchar](18) NULL,
	[CLIE_DATA_CADASTRO] [datetime] DEFAULT GETDATE(),
	[CLIE_STATUS] [int] NOT NULL,
	[CLIE_EXCLUIDO] [bit] NOT NULL,
);
Go
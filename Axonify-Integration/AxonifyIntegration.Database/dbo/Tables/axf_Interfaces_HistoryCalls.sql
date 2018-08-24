CREATE TABLE [dbo].[axf_Interfaces_HistoryCalls](
	[Hid] [bigint] IDENTITY(1,1) NOT NULL,
	[InterfaceId] [varchar](80) NOT NULL,
	[StartExecutionDate] [datetime] NOT NULL,
	[EndExecutionDate] [datetime] NULL,
	[IsSuccess] [bit] NOT NULL,
	[ErrorMessage] [varchar](max) NULL,
 CONSTRAINT [PK_axf_Interfaces_HistoryCalls] PRIMARY KEY CLUSTERED 
(
	[Hid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[axf_Interfaces_HistoryCalls] ADD  CONSTRAINT [DF_axf_Interfaces_HistoryCalls_StartExecutionDate]  DEFAULT (getdate()) FOR [StartExecutionDate]
GO
ALTER TABLE [dbo].[axf_Interfaces_HistoryCalls] ADD  CONSTRAINT [DF_axf_Interfaces_HistoryCalls_IsSuccess]  DEFAULT ((0)) FOR [IsSuccess]
這是一個使用 ASP.NET Core MVC 架構所開發的任務追蹤系統，用於每日、每週、每月與不定期任務的管理。專案整合了 Entity Framework Core、SQL Server Agent 與 SignalR，以實現定時重設與即時更新等功能。

功能

支援新增、編輯與刪除任務
使用 SQL Server Agent 每分鐘檢查任務重設時間

使用 SignalR 即時更新前端畫面
任務依類型分為每日、每週、每月與不定期
使用者登入後可查看自己任務列表

技術

ASP.NET Core MVC
Entity Framework Core
SQL Server (含 Agent 定時排程)
SignalR (即時雙向通訊)
jQuery (前端互動)

使用方法

登入帳號
點選任務類型（每日/每週/每月/不定期）(目前僅有每日功能)
可進行新增、修改或刪除
設定重置時間
如果有項目為以勾選狀態，則會依據設置的重置時間重置為未勾選狀態

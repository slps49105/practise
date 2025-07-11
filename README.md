<h1>簡易備忘錄系統（含會員登入功能）</h1>
簡易的備忘錄系統，使用者可註冊登入，建立、修改、刪除備忘事項，透過勾選控制完成狀態，且同步更新至資料庫。


<h2>實作功能：</h2>
1.會員系統(註冊/登入/登出，使用cookie)<br>
2.備忘錄CRUD(建立、查詢、修改、刪除)<br>
3.AJAX與SignalR完成狀態即時更新<br>


<h2>使用技術：</h2>
前端：HTML+CSS+jQuery+AJAX<br>
後端：ASP.NET Core MVC(C#)+MS SQL(含 Stored Procedure、SQL Server Agent)<br>
其他：SignalR<br>


<h2>畫面截圖：</h2>
<h3>登入畫面</h3>
使用 Cookie-based Authentication 實作登入功能。登入成功後，驗證資訊將寫入 Cookie，透過 Cookie 識別使用者身分並控管登入狀態。
<img width="1183" height="557" alt="螢幕擷取畫面 2025-07-10 123510" src="https://github.com/user-attachments/assets/2c73f45c-d6a3-4f34-9d52-340adcde0a90" />

<h3>主畫面(上)</h3>
分為上半部與下半部，上半部顯示時間，下半部則為主要功能。
<img width="1174" height="500" alt="螢幕擷取畫面 2025-07-10 124657" src="https://github.com/user-attachments/assets/06e1186b-e024-475b-a571-b2b7a96f5890" />

<h3>主畫面(下)</h3>
有附帶月曆方便確認日期。
<img width="1164" height="482" alt="螢幕擷取畫面 2025-07-10 124816" src="https://github.com/user-attachments/assets/a42d5ed8-6833-4aa8-8df7-cd6a209e7a28" />


<h2>操作說明：</h2>
<h3>新增</h3>
<img width="92" height="36" alt="螢幕擷取畫面 2025-07-10 125017" src="https://github.com/user-attachments/assets/42e4c0b5-ae0e-4347-bd94-933b74c3ed77" /><br><br>
點選新增按鈕後便會跳出項目名稱輸入框以及時間選擇器。

<img width="466" height="46" alt="螢幕擷取畫面 2025-07-10 125327" src="https://github.com/user-attachments/assets/e1111dd9-515d-452b-b02e-10d3a1ed127c" /><br><br>
按下Confirm後，便會將資料新增至資料庫並同步更新畫面。

<img width="354" height="74" alt="螢幕擷取畫面 2025-07-10 125353" src="https://github.com/user-attachments/assets/9f46291d-918e-437a-8831-ef168ea80070" /><br><br>
若為勾選狀態，右側時間到時，將重置為未勾選狀態。

<img width="138" height="38" alt="螢幕擷取畫面 2025-07-10 130132" src="https://github.com/user-attachments/assets/4efb2471-4843-4c89-87aa-64f8e5f1537a" /><img width="64" height="40" alt="螢幕擷取畫面 2025-07-10 130214" src="https://github.com/user-attachments/assets/ee594eb2-207d-4cbd-88e0-77885077a247" />

<img width="133" height="28" alt="螢幕擷取畫面 2025-07-10 130309" src="https://github.com/user-attachments/assets/fc9e2ea9-aa48-4949-b721-db2142f81a81" /><img width="64" height="39" alt="螢幕擷取畫面 2025-07-10 130321" src="https://github.com/user-attachments/assets/6cfc0cb8-c68e-461d-83e7-0e07de064037" />


<h3>編輯</h3>
<img width="29" height="29" alt="螢幕擷取畫面 2025-07-10 130429" src="https://github.com/user-attachments/assets/4b6bc81e-cffd-4e95-b8a3-a708e1e6f08f" /><br><br>
編輯按鈕點選後，項目將變為可更改狀態。

<img width="462" height="37" alt="螢幕擷取畫面 2025-07-10 130611" src="https://github.com/user-attachments/assets/efcdd0f3-04c5-49dd-8fa1-228a6b5899df" /><br><br>
按下Confirm後將更新狀態。

<img width="136" height="28" alt="螢幕擷取畫面 2025-07-10 130714" src="https://github.com/user-attachments/assets/f607dce7-cb9c-4d32-a383-26ab74154765" /><br><br>


<h3>刪除</h3>
<img width="23" height="25" alt="螢幕擷取畫面 2025-07-10 130812" src="https://github.com/user-attachments/assets/b614a922-37dc-4463-add2-f96fb70e3e50" /><br><br>
刪除按鈕點選後，項目將直接從畫面上以及資料庫移除。


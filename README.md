
# 💳 Algebra Bankica – Internal Banking Web App

**Algebra Bankica** is a modern digital application developed for the students and staff of **Algebra University College (Bernays)**.  
The application enables secure and easy management of personal finances within the university environment.

Through the web interface, users can:
- Securely execute transactions
- Scan QR codes for quick payments
- Manage their cards and financial information
- Track transaction history with visual charts
- Manage profile preferences and security options

---

## 🌐 Frontend – React Client

### ✅ Requirements

- Node.js (v18+ recommended)
- Git

> You do not need any prior React knowledge.

### ▶️ How to Run the App Locally

```bash
git clone https://github.com/mipar52/pra-project-internal-banking.git
cd pra-project-internal-banking/react-client-internal-banking
npm install
npm run dev
```

Then visit the local URL (e.g., `http://localhost:5173`) in your browser.  
⚠️ Ensure the backend is running (see backend setup below).

---

### 📚 Libraries Used

**Core Libraries**
- `react` – UI component library
- `react-dom` – DOM rendering
- `react-router-dom` – Page routing
- `vite` – Fast build tool
- `typescript` – Static type checking

**UI & Utility**
- `bootstrap`, `react-bootstrap` – Styling and components
- `react-icons` – Icon set
- `axios` – API communication
- `html5-qrcode`, `react-qrcode` – QR code scanning/rendering
- `recharts` – Charting and visualizations

**Development Tools**
- `eslint`, `eslint-plugin-*` – Linting and quality
- `@types/*` – TypeScript definitions
- `vite-plugin-react` – React/Vite integration

---

### 🗂️ Project Structure

```plaintext
react-client-internal-banking/
├── public/                    # Static files
├── src/
│   ├── assets/                # Images/assets
│   ├── axiosHelper/           # Axios config
│   ├── css/                   # Scoped styles
│   ├── hooks/                 # Custom React hooks
│   ├── router/                # Page components
│   ├── App.tsx                # App entry
│   ├── main.tsx               # Render entry
│   └── index.css              # Global CSS
├── package.json               # NPM metadata
├── vite.config.ts             # Vite settings
├── tsconfig*.json             # TypeScript config
└── README.md                  # Project doc
```

---

### 📺 Key Screens

Located in `src/router/`:

- `Login.tsx` – Login screen  
- `Dashboard.tsx` – Main dashboard  
- `EnterManually.tsx` – Manual transaction entry  
- `QrScanner.tsx` – QR code scanning  
- `SelectCard.tsx` – Choose payment card  
- `TransactionDetails.tsx` – Transaction overview  
- `History.tsx` – Transaction history with charts  
- `Settings.tsx` – User preferences  
- `MyProfile.tsx` – Profile page  
- `AllFriends.tsx` – Bank contacts  
- `RequestMoney.tsx` – Request funds

---

### 📝 Additional Notes

- All API requests go through: `src/axiosHelper/axiosInstance.ts`
- Components like `SuccessPopup.tsx` and `ErrorPopup.tsx` are reusable.
- CSS is scoped per screen/component.

---

## 🧠 Backend – ASP.NET Core Web API (C#)

### ✅ Requirements

- Visual Studio 2022
- ASP.NET Core Web API tools

> No prior knowledge required.

### ▶️ How to Run the Backend Locally

```bash
git clone https://github.com/mipar52/pra-project-internal-banking.git
```

1. Open the solution file:
   ```
   pra-project-internal-banking/backend/PRA_PROJECT/PRA_PROJECT.snl
   ```
2. In Visual Studio:
   - Open `Dependencies`
   - Right-click `Packages` and select `Update...`
   - Press **Run**

⚠️ Make sure the database is set up beforehand (see below).

---

### 🧰 Frameworks Used

- `Microsoft.AspNetCore.App`
- `Microsoft.NETCore.App`

### 📦 Packages Used

- `MailKit`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.AspNetCore.Authentication.OpenIdConnect`
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Swashbuckle.AspNetCore`
- `Twilio`

### 🛠️ UI & Utility

- 2FA Code Provider
- CvvHashProvider
- JwtTokenProvider
- PasswordHashProvider
- `xUnit` for unit testing

---

### 🗂️ Project Structure

```plaintext
PRA_Project.sln
PRA_Project/
├── Controllers/
├── DTOs/
├── Models/
├── ProfilePictureRepo/
├── Security/
├── Service/
└── TestProject/
```

---

## 🗄️ Database Setup

### ✅ Requirements

Install **SQL Server Management Studio (SSMS)**  
➡️ [Download SSMS](https://learn.microsoft.com/hr-hr/ssms/install/install)

### ▶️ Running the SQL Script

1. Open SSMS and connect to your local server.
2. Load the script:
   ```
   pra-project-internal-banking/database/Database.sql
   ```
3. Click **Execute** or press **F5**.

This will:
- Create the database
- Populate it with demo/test data

✅ Done! Now you can fully run both the backend and frontend!


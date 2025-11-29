// wwwroot/js/firebaseConfig.js
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-analytics.js";
import { getStorage } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-storage.js";

// Cấu hình Firebase của bạn
const firebaseConfig = {
    apiKey: "AIzaSyAPoW_YHRpeV3S5IfU75c2mPZLUVN5h1_o",
    authDomain: "ho-tro-hoc-tap-1bf1f.firebaseapp.com",
    projectId: "ho-tro-hoc-tap-1bf1f",
    storageBucket: "ho-tro-hoc-tap-1bf1f.firebasestorage.app",
    messagingSenderId: "776893455274",
    appId: "1:776893455274:web:84536ab780a29d14e7e9d3",
    measurementId: "G-70838HXZZJ"
};

// Khởi tạo Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);
const storage = getStorage(app);

export { app, analytics, storage };

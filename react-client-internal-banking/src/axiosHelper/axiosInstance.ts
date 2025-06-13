// src/utils/axiosInstance.ts
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "http://localhost:5026/api",
});

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.clear(); // Remove token + user info
      window.location.href = "/"; // Redirect to login
    }
    return Promise.reject(error);
  }
);

export default axiosInstance;

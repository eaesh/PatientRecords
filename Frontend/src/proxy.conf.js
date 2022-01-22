const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/api/patients"
    ],
    target: "https://localhost:5001",
    secure: false
  }
]

module.exports = PROXY_CONFIG;

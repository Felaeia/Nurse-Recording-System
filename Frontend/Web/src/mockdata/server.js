// server.js (Corrected)

const jsonServer = require('json-server')
const server = jsonServer.create()
const router = jsonServer.router('data.json')
const middlewares = jsonServer.defaults()

// Use default middlewares (logger, static, cors and no-cache)
server.use(middlewares)

// ------------------------------------------------------------------
// IMPORTANT: Add the body parser middleware *BEFORE* your custom routes.
// jsonServer.bodyParser is needed to parse the JSON body for POST requests.
server.use(jsonServer.bodyParser)
// ------------------------------------------------------------------

// Add custom routes before JSON Server router
server.post('/login', (req, res) => {
  // Now, req.body should contain { username: '...', password: '...' }
  const { username, password } = req.body // Get the mock nurse data from the database
  const db = router.db // lowdb instance
  const mockNurse = db.get('nurse').value()[0] // Assuming only one nurse object

  if (mockNurse && mockNurse.username === username && mockNurse.password === password) {
    // Successful login
    res.jsonp({
      success: true,
      nurse: {
        id: mockNurse.id,
        username: mockNurse.username,
        email: mockNurse.email,
      },
    })
  } else {
    // Failed login
    res.status(401).jsonp({ success: false, message: 'Invalid username or password' })
  }
})

// Use the default JSON Server router for other endpoints
server.use(router)

// Start the server on port 3000
const PORT = 3000
server.listen(PORT, () => {
  console.log(`JSON Server is running at http://localhost:${PORT}`)
  console.log('Endpoints: /nurse, /patients, /patientRecords. /appointments')
  console.log('Custom Login: POST /login')
})

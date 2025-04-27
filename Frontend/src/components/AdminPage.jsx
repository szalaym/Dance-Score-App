import React, { useState } from 'react';
import './AdminPage.css';

const AdminPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [versenyNev, setVersenyNev] = useState('');
  const [versenyIdopont, setVersenyIdopont] = useState('');
  const [selectedCategories, setSelectedCategories] = useState([]); // Kiválasztott kategóriák tárolása
  const [message, setMessage] = useState('');

  // Elérhető kategóriák listája (ezt testreszabhatod a saját igényeid szerint)
  const availableCategories = ['Junior', 'Senior', 'Felnőtt', 'Gyermek', 'Profi'];

  const handleLogin = () => {
    if (username === 'admin' && password === 'admin123') {
      setIsAuthenticated(true);
      setMessage('Sikeres bejelentkezés!');
    } else {
      setMessage('Hibás felhasználónév vagy jelszó!');
    }
  };

  const handleCategoryChange = (e) => {
    // A kiválasztott opciók átalakítása tömbbé
    const selectedOptions = Array.from(e.target.selectedOptions, option => option.value);
    setSelectedCategories(selectedOptions);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Ellenőrizzük, hogy van-e kiválasztott kategória
    if (selectedCategories.length === 0) {
      setMessage('Kérlek, válassz legalább egy kategóriát!');
      return;
    }

    const competitionData = {
      Nev: versenyNev,
      Idopont: versenyIdopont,
      Kategoriak: selectedCategories, // Tömbként küldjük, nem stringként
    };

    try {
      const response = await fetch('https://localhost:44333/api/Verseny', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(competitionData),
      });

      if (response.ok) {
        const result = await response.json();
        setMessage('Verseny sikeresen hozzáadva! ' + result);
        setVersenyNev('');
        setVersenyIdopont('');
        setSelectedCategories([]); // Kategóriák visszaállítása üres tömbre
      } else {
        const errorText = await response.text();
        setMessage(`Hiba a verseny hozzáadása közben: ${response.status} - ${errorText}`);
      }
    } catch (error) {
      setMessage('Hiba a szerverrel való kommunikációban: ' + error.message);
    }
  };

  if (!isAuthenticated) {
    return (
      <div className="admin-login">
        <h2>Admin Bejelentkezés</h2>
        <input
          type="text"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          placeholder="Felhasználónév"
          style={{ padding: '10px', margin: '5px', width: '200px' }}
        />
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Jelszó"
          style={{ padding: '10px', margin: '5px', width: '200px' }}
        />
        <button onClick={handleLogin} style={{ padding: '10px 20px', margin: '5px' }}>
          Bejelentkezés
        </button>
        <p>{message}</p>
      </div>
    );
  }

  return (
    <div className="admin-page">
      <h2>Új Verseny Hozzáadása</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          value={versenyNev}
          onChange={(e) => setVersenyNev(e.target.value)}
          placeholder="Verseny neve"
          required
          style={{ padding: '10px', margin: '5px', width: '300px' }}
        />
        <input
          type="datetime-local"
          value={versenyIdopont}
          onChange={(e) => setVersenyIdopont(e.target.value)}
          required
          style={{ padding: '10px', margin: '5px', width: '300px' }}
        />
        <div style={{ margin: '5px' }}>
          <label htmlFor="categories">Kategóriák (többet is választhatsz, tartsd lenyomva a Ctrl-t):</label>
          <select
            id="categories"
            multiple
            value={selectedCategories}
            onChange={handleCategoryChange}
            style={{ padding: '10px', margin: '5px', width: '300px', height: '100px' }}
            required
          >
            {availableCategories.map((category) => (
              <option key={category} value={category}>
                {category}
              </option>
            ))}
          </select>
          <p>Kiválasztott kategóriák: {selectedCategories.join(', ') || 'Nincs kiválasztva'}</p>
        </div>
        <button type="submit" style={{ padding: '10px 20px', margin: '5px' }}>
          Verseny mentése
        </button>
      </form>
      <p>{message}</p>
    </div>
  );
};

export default AdminPage;
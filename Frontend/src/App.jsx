import React, { useState, useEffect } from "react";
import { Navigation } from "./components/navigation";
import Header from "./components/header";
import { Features } from "./components/features";
import { About } from "./components/about";
import { Services } from "./components/services";
import { Contact } from "./components/contact";
import Modal from "./components/Modal";
import AdminPage from "./components/AdminPage";
import JsonData from "./data/data.json";
import SmoothScroll from "smooth-scroll";

// Error Boundary komponens
class ErrorBoundary extends React.Component {
  state = { hasError: false, error: null };

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    console.error("Hiba történt a renderelés során:", error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div style={{ padding: "20px", textAlign: "center" }}>
          <h1>Hiba történt</h1>
          <p>Valami nem működik megfelelően. Kérlek, próbáld újra később.</p>
          <p>Hibaüzenet: {this.state.error?.message}</p>
          <button onClick={() => window.location.reload()}>Oldal újratöltése</button>
        </div>
      );
    }
    return this.props.children;
  }
}

export const scroll = new SmoothScroll('a[href*="#"]', {
  speed: 800,
  speedAsDuration: true,
  easing: "easeInOutCubic",
  updateURL: true,
  offset: 0,
});

/* eslint-disable */

const App = () => {
  const [landingPageData, setLandingPageData] = useState({});
  const [showModal, setShowModal] = useState(false);
  const [showResultsModal, setShowResultsModal] = useState(false);
  const [showRegistrationModal, setShowRegistrationModal] = useState(false);
  const [teamName, setTeamName] = useState("");
  const [teamSize, setTeamSize] = useState(0);
  const [category, setCategory] = useState("");
  const [members, setMembers] = useState([]);
  const [newMember, setNewMember] = useState("");
  const [competition, setCompetition] = useState("");
  const [zeneLink, setZeneLink] = useState("");
  const [results, setResults] = useState([]);
  const [filteredResults, setFilteredResults] = useState([]);
  const [filterCompetition, setFilterCompetition] = useState("");
  const [filterCategory, setFilterCategory] = useState("");
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [password, setPassword] = useState("");
  const [secretKey, setSecretKey] = useState("");
  const [message, setMessage] = useState("");
  const [availableCompetitions, setAvailableCompetitions] = useState([]);
  const [availableCategoriesForRegistration, setAvailableCategoriesForRegistration] = useState([]);
  const [availableCategoriesForResults, setAvailableCategoriesForResults] = useState([]);
  const [currentPath, setCurrentPath] = useState(window.location.pathname);

  useEffect(() => {
    setLandingPageData(JsonData);
    fetchAvailableCompetitions();
    fetchResults();
  }, []);

  useEffect(() => {
    setFilteredResults(results);
  }, [results]);

  useEffect(() => {
    try {
      console.log("competition értéke:", competition);
      console.log("availableCompetitions:", availableCompetitions);
      if (competition && availableCompetitions.length > 0) {
        const normalizedCompetition = competition.trim();
        const selectedCompetition = availableCompetitions.find(comp => comp.Nev.trim() === normalizedCompetition);
        console.log("selectedCompetition (Nevezés):", selectedCompetition);
        if (selectedCompetition) {
          let categories = [];
          if (typeof selectedCompetition.Kategoriak === 'string') {
            categories = selectedCompetition.Kategoriak.split(',').map(cat => cat.trim());
          } else if (Array.isArray(selectedCompetition.Kategoriak)) {
            categories = selectedCompetition.Kategoriak.map(cat => cat.trim());
          } else {
            console.warn(`A Kategoriak mező nem string és nem tömb (Nevezés): ${selectedCompetition.Kategoriak}`);
          }
          console.log("Betöltött kategóriák (Nevezés):", categories);
          setAvailableCategoriesForRegistration(categories);
          setCategory("");
        } else {
          setAvailableCategoriesForRegistration([]);
          setCategory("");
          console.warn(`Nem található verseny (Nevezés): ${normalizedCompetition}`);
        }
      } else {
        setAvailableCategoriesForRegistration([]);
        setCategory("");
      }
    } catch (error) {
      console.error("Hiba történt a kategóriák betöltésekor (Nevezés):", error);
      setAvailableCategoriesForRegistration([]);
      setCategory("");
    }
  }, [competition, availableCompetitions]);

  useEffect(() => {
    try {
      console.log("filterCompetition értéke:", filterCompetition);
      console.log("availableCompetitions:", availableCompetitions);
      if (filterCompetition && availableCompetitions.length > 0) {
        const normalizedFilterCompetition = filterCompetition.trim();
        const selectedCompetition = availableCompetitions.find(comp => comp.Nev.trim() === normalizedFilterCompetition);
        console.log("selectedCompetition (Eredmények):", selectedCompetition);
        if (selectedCompetition) {
          let categories = [];
          if (typeof selectedCompetition.Kategoriak === 'string') {
            categories = selectedCompetition.Kategoriak.split(',').map(cat => cat.trim());
          } else if (Array.isArray(selectedCompetition.Kategoriak)) {
            categories = selectedCompetition.Kategoriak.map(cat => cat.trim());
          } else {
            console.warn(`A Kategoriak mező nem string és nem tömb (Eredmények): ${selectedCompetition.Kategoriak}`);
          }
          console.log("Betöltött kategóriák (Eredmények):", categories);
          setAvailableCategoriesForResults(categories);
          setFilterCategory("");
        } else {
          setAvailableCategoriesForResults([]);
          setFilterCategory("");
          console.warn(`Nem található verseny (Eredmények): ${normalizedFilterCompetition}`);
        }
      } else {
        setAvailableCategoriesForResults([]);
        setFilterCategory("");
      }
    } catch (error) {
      console.error("Hiba történt a kategóriák betöltésekor (Eredmények):", error);
      setAvailableCategoriesForResults([]);
      setFilterCategory("");
    }
  }, [filterCompetition, availableCompetitions]);

  const fetchAvailableCompetitions = async () => {
    try {
      console.log("Versenyek lekérdezése indul: https://localhost:44333/api/Verseny");
      const response = await fetch("https://localhost:44333/api/Verseny", {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          "Accept": "application/json"
        },
        mode: "cors"
      });
      console.log("Válasz státusza:", response.status);
      if (response.ok) {
        const data = await response.json();
        console.log("Versenyek betöltve (nyers adat):", data);
        // Az új adatstruktúrához igazítjuk
        if (Array.isArray(data)) {
          const mappedData = data.map(item => ({
            Id: item.id, // Kisbetűs "id" -> Nagybetűs "Id"
            Nev: item.nev, // Kisbetűs "nev" -> Nagybetűs "Nev"
            Idopont: item.idopont, // Kisbetűs "idopont" -> Nagybetűs "Idopont"
            Kategoriak: Array.isArray(item.kategoriak) ? item.kategoriak.join(", ") : item.kategoriak // Tömb -> String
          }));
          console.log("Feldolgozott versenyek:", mappedData);
          setAvailableCompetitions(mappedData);
          if (mappedData.length === 0) {
            setMessage("Nincsenek elérhető versenyek.");
          }
        } else {
          setMessage("A kapott adat nem megfelelő formátumú (nem tömb).");
          console.error("A kapott adat nem tömb:", data);
        }
      } else {
        const errorText = await response.text();
        setMessage(`Hiba a versenyek lekérdezésekor: ${response.status} - ${response.statusText}`);
        console.error("Hiba a versenyek lekérdezésekor, státusz:", response.status, "Szöveg:", errorText);
      }
    } catch (error) {
      setMessage(`Hiba történt a versenyek betöltésekor: ${error.message}`);
      console.error("Hiba a versenyek betöltésekor:", error);
    }
  };

  const fetchResults = async () => {
    try {
      const encodedVerseny = encodeURIComponent(filterCompetition || "");
      const encodedKategoria = encodeURIComponent(filterCategory || "");
      const url = `https://localhost:44333/api/Nevezes?verseny=${encodedVerseny}&kategoria=${encodedKategoria}`;
      console.log("Eredmények lekérdezése indul:", url);
      const response = await fetch(url, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          "Accept": "application/json"
        },
        mode: "cors"
      });
      console.log("Eredmények válasz státusza:", response.status);
      if (response.ok) {
        const data = await response.json();
        console.log("Eredmények betöltve (nyers adat):", data);
        if (Array.isArray(data)) {
          // Az új adatstruktúrához igazítjuk
          const mappedData = data.map(item => ({
            Id: item.id || item.Id,
            Verseny: item.verseny || item.Verseny,
            Kategoria: item.kategoria || item.Kategoria,
            CsapatNev: item.csapatNev || item.CsapatNev,
            Pontszam: item.pontszam || item.Pontszam
          }));
          console.log("Feldolgozott eredmények:", mappedData);
          setResults(mappedData);
          setFilteredResults(mappedData);
          setMessage(mappedData.length > 0 ? "Sikeres betöltés" : "Nincs megjeleníthető adat.");
        } else {
          setMessage("A kapott eredmények nem megfelelő formátumúak (nem tömb).");
          console.error("A kapott eredmények nem tömb:", data);
        }
      } else {
        const errorText = await response.text();
        setMessage(`Hiba az eredmények lekérdezésekor: ${response.status} - ${response.statusText}`);
        console.error("Hiba az eredmények lekérdezésekor, státusz:", response.status, "Szöveg:", errorText);
      }
    } catch (error) {
      setMessage(`Hiba történt az eredmények betöltésekor: ${error.message}`);
      console.error("Hiba az eredmények betöltésekor:", error);
    }
  };

  const handleModalOpen = () => setShowModal(true);
  const handleModalClose = () => {
    setShowModal(false);
    setMessage("");
    setCompetition("");
    setCategory("");
    setAvailableCategoriesForRegistration([]);
  };

  const handleResultsModalOpen = () => {
    setShowResultsModal(true);
    fetchResults();
  };

  const handleResultsModalClose = () => {
    setShowResultsModal(false);
    setMessage("");
    setFilterCompetition("");
    setFilterCategory("");
    setAvailableCategoriesForResults([]);
  };

  const handleRegistrationModalOpen = () => setShowRegistrationModal(true);
  const handleRegistrationModalClose = () => {
    setShowRegistrationModal(false);
    setMessage("");
  };

  const handleAddMember = () => {
    if (newMember.trim() === "") {
      setMessage("Kérlek, add meg a tag nevét!");
      return;
    }

    if (members.length >= teamSize) {
      setMessage(`Nem adhatsz hozzá több tagot, a létszám ${teamSize} fő!`);
      return;
    }

    setMembers([...members, newMember.trim()]);
    setNewMember("");
    setMessage("");
  };

  const handleRemoveMember = (index) => {
    setMembers(members.filter((_, i) => i !== index));
    setMessage("");
  };

  const handleCompetitionChange = (e) => {
    const newCompetition = e.target.value;
    console.log("Új competition érték:", newCompetition);
    setCompetition(newCompetition);
  };

  const handleFilterCompetitionChange = (e) => {
    const newFilterCompetition = e.target.value;
    console.log("Új filterCompetition érték:", newFilterCompetition);
    setFilterCompetition(newFilterCompetition);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!teamName.trim()) {
      setMessage("A csapatnév nem lehet üres!");
      return;
    }

    if (!competition) {
      setMessage("Kérlek, válassz egy versenyt!");
      return;
    }

    if (teamSize <= 0) {
      setMessage("A létszámnak nagyobbnak kell lennie, mint 0!");
      return;
    }

    if (!category) {
      setMessage("Kérlek, válassz egy kategóriát!");
      return;
    }

    if (members.length !== teamSize) {
      setMessage(`A tagok száma (${members.length}) nem egyezik a megadott létszámmal (${teamSize})!`);
      return;
    }

    const data = {
      CsapatNev: teamName,
      Letszam: teamSize,
      Kategoria: category,
      Verseny: competition,
      ZeneLink: zeneLink || null,
      Tagok: members,
    };

    try {
      const response = await fetch('https://localhost:44333/api/Csapat', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        const result = await response.json();
        setMessage(`Csapat sikeresen regisztrálva! Csapat ID: ${result}`);
        setTeamName("");
        setTeamSize(0);
        setCategory("");
        setCompetition("");
        setZeneLink("");
        setMembers([]);
        setAvailableCategoriesForRegistration([]);
      } else {
        const errorData = await response.json();
        setMessage(`Hiba történt: ${errorData.Message || 'Ismeretlen hiba'}`);
      }
    } catch (error) {
      setMessage(`Hiba történt: ${error.message}`);
    }
  };

  const handleRegistrationSubmit = async (e) => {
    e.preventDefault();

    const data = {
      Nev: name,
      Email: email,
      Telefon: phone,
      Jelszo: password,
      TitkosKulcs: secretKey,
    };

    try {
      const response = await fetch('https://localhost:44333/api/Birok', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        const result = await response.json();
        setMessage(`Bírói profil sikeresen regisztrálva! Bíró ID: ${result}`);
        setName("");
        setEmail("");
        setPhone("");
        setPassword("");
        setSecretKey("");
      } else {
        const errorData = await response.json();
        setMessage(`Hiba történt: ${errorData.Message || 'Ismeretlen hiba'}`);
      }
    } catch (error) {
      setMessage(`Hiba történt: ${error.message}`);
    }
  };

  if (currentPath === '/admin/secret-page') {
    return <AdminPage />;
  }

  return (
    <ErrorBoundary>
      <div>
        <Navigation 
          onOpenModal={handleModalOpen} 
          onOpenResultsModal={handleResultsModalOpen} 
          onOpenRegistrationModal={handleRegistrationModalOpen} 
        />
        <Header data={landingPageData.Header} />
        <Features data={landingPageData.Features} />
        <About data={landingPageData.About} />
        <Services data={landingPageData.Services} />
        <Contact data={landingPageData.Contact} />
        <Modal show={showModal} onClose={handleModalClose}>
          <h2>Nevezési Űrlap</h2>
          {message && <p style={{ color: message.includes('Hiba') ? 'red' : 'green', marginBottom: '15px' }}>{message}</p>}
          <form onSubmit={handleSubmit} style={{ maxHeight: "80vh", overflowY: "auto", paddingRight: "10px" }}>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="teamName">Csapatnév</label>
              <input
                type="text"
                id="teamName"
                value={teamName}
                onChange={(e) => setTeamName(e.target.value)}
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              />
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="competition">Verseny</label>
              <select
                id="competition"
                value={competition}
                onChange={handleCompetitionChange}
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              >
                <option value="">Válassz versenyt</option>
                {availableCompetitions.map((comp) => (
                  <option key={comp.Id} value={comp.Nev}>{comp.Nev}</option>
                ))}
              </select>
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="category">Kategória</label>
              <select
                id="category"
                value={category}
                onChange={(e) => setCategory(e.target.value)}
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              >
                <option value="">Válassz kategóriát</option>
                {availableCategoriesForRegistration.map((cat, index) => (
                  <option key={index} value={cat}>{cat}</option>
                ))}
              </select>
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="teamSize">Létszám</label>
              <input
                type="number"
                id="teamSize"
                value={teamSize}
                onChange={(e) => setTeamSize(parseInt(e.target.value) || 0)}
                min="1"
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              />
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="members">Tagok</label>
              <br />
              <div style={{ display: "flex", alignItems: "center" }}>
                <input
                  type="text"
                  id="newMember"
                  value={newMember}
                  onChange={(e) => setNewMember(e.target.value)}
                  style={{ width: "80%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                />
                <button
                  type="button"
                  onClick={handleAddMember}
                  style={{
                    marginLeft: "10px",
                    padding: "10px",
                    backgroundColor: members.length >= teamSize ? "#ccc" : "#4CAF50",
                    color: "white",
                    border: "none",
                    borderRadius: "5px",
                    cursor: members.length >= teamSize ? "not-allowed" : "pointer",
                  }}
                  disabled={members.length >= teamSize}
                >
                  +
                </button>
              </div>
              <ul>
                {members.map((member, index) => (
                  <li key={index} style={{ display: "flex", alignItems: "center", justifyContent: "space-between", padding: "5px 0" }}>
                    {member}
                    <button
                      type="button"
                      onClick={() => handleRemoveMember(index)}
                      style={{ marginLeft: "10px", padding: "5px 10px", backgroundColor: "#FF4C4C", color: "white", border: "none", borderRadius: "5px", cursor: "pointer" }}
                    >
                      X
                    </button>
                  </li>
                ))}
              </ul>
              <p>Jelenlegi tagok száma: {members.length} / {teamSize}</p>
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="zeneLink">Zene linkje (opcionális)</label>
              <input
                type="url"
                id="zeneLink"
                value={zeneLink}
                onChange={(e) => setZeneLink(e.target.value)}
                placeholder="https://example.com/zene.mp3"
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
              />
            </div>
            <button
              type="submit"
              style={{ backgroundColor: "#008CBA", color: "white", padding: "10px 20px", border: "none", borderRadius: "5px", cursor: "pointer" }}
            >
              Nevezés
            </button>
          </form>
        </Modal>
        <Modal show={showResultsModal} onClose={handleResultsModalClose}>
          <h2>Eredmények</h2>
          {message && <p style={{ color: message.includes('Hiba') ? 'red' : 'green', marginBottom: '15px' }}>{message}</p>}
          <div style={{ marginBottom: "15px" }}>
            <label htmlFor="filterCompetition">Verseny</label>
            <select
              id="filterCompetition"
              value={filterCompetition}
              onChange={handleFilterCompetitionChange}
              style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
            >
              <option value="">Összes verseny</option>
              {availableCompetitions.map((comp) => (
                <option key={comp.Id} value={comp.Nev}>{comp.Nev}</option>
              ))}
            </select>
          </div>
          <div style={{ marginBottom: "15px" }}>
            <label htmlFor="filterCategory">Kategória</label>
            <select
              id="filterCategory"
              value={filterCategory}
              onChange={(e) => setFilterCategory(e.target.value)}
              style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
            >
              <option value="">Összes kategória</option>
              {availableCategoriesForResults.map((cat, index) => (
                <option key={index} value={cat}>{cat}</option>
              ))}
            </select>
          </div>
          <button
            onClick={fetchResults}
            style={{ backgroundColor: "#008CBA", color: "white", padding: "10px 20px", border: "none", borderRadius: "5px", cursor: "pointer", marginBottom: "10px" }}
          >
            Frissítés
          </button>
          {filteredResults.length > 0 ? (
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
              <thead>
                <tr>
                  <th style={{ border: "1px solid #ccc", padding: "10px" }}>Verseny</th>
                  <th style={{ border: "1px solid #ccc", padding: "10px" }}>Kategória</th>
                  <th style={{ border: "1px solid #ccc", padding: "10px" }}>Csapatnév</th>
                  <th style={{ border: "1px solid #ccc", padding: "10px" }}>Pontszám</th>
                </tr>
              </thead>
              <tbody>
                {filteredResults.map((result) => (
                  <tr key={result.Id}>
                    <td style={{ border: "1px solid #ccc", padding: "10px" }}>{result.Verseny}</td>
                    <td style={{ border: "1px solid #ccc", padding: "10px" }}>{result.Kategoria}</td>
                    <td style={{ border: "1px solid #ccc", padding: "10px" }}>{result.CsapatNev}</td>
                    <td style={{ border: "1px solid #ccc", padding: "10px" }}>{result.Pontszam}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <p>Nincs megjeleníthető adat.</p>
          )}
        </Modal>
        <Modal show={showRegistrationModal} onClose={handleRegistrationModalClose}>
          <h2>Bírói <br />Regisztráció</h2>
          {message && <p style={{ color: message.includes('Hiba') ? 'red' : 'green', marginBottom: '15px' }}>{message}</p>}
          <form onSubmit={handleRegistrationSubmit} style={{ maxHeight: "80vh", overflowY: "auto", paddingRight: "10px" }}>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="name">Név</label>
              <input
                type="text"
                id="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              />
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="email">Email cím</label>
              <input
                type="email"
                id="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              />
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="phone">Telefonszám</label>
              <input
                type="tel"
                id="phone"
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              />
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="password">Jelszó</label>
              <input
                type="password"
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              />
            </div>
            <div style={{ marginBottom: "15px" }}>
              <label htmlFor="secretKey">Bíró azonosító</label>
              <input
                type="text"
                id="secretKey"
                value={secretKey}
                onChange={(e) => setSecretKey(e.target.value)}
                style={{ width: "100%", padding: "10px", borderRadius: "5px", border: "1px solid #ccc" }}
                required
              />
            </div>
            <button
              type="submit"
              style={{ backgroundColor: "#008CBA", color: "white", padding: "10px 20px", border: "none", borderRadius: "5px", cursor: "pointer" }}
            >
              Regisztráció
            </button>
          </form>
        </Modal>
      </div>
    </ErrorBoundary>
  );
};

export default App;
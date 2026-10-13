# Municipal Citizen Portal

An AAPD module, C# desktop application that lets residents report
municipal issues directly to their local government.

**Part 1** implemented the **Report Issues**.
**Part 2** adds **Local Events and Announcements**: searchable, date-ordered
events, urgent-first announcements and a recommendation feature built on
stacks, queues, priority queues, hash tables, dictionaries, sorted
dictionaries and sets (sections 5 and 6 below).
**Service Request Status** is still a disabled placeholder on the main menu,
ready for Task 3.

\---

## 1\. Requirements

* Windows 10/11
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (Desktop pack, which ships automatically with the SDK on Windows)
* Visual Studio 2022 (17.8+) is recommended but not required — the project
can also be built and run entirely from the command line.

> **Note:** Windows Forms only runs on Windows. The project targets
> `net8.0-windows` and will not build or run on macOS/Linux.

\---

## 2\. Project structure

```
MunicipalReportingApp/
├── MunicipalReportingApp.sln
└── MunicipalReportingApp/
    ├── MunicipalReportingApp.csproj
    ├── Program.cs                    
    ├── Common/
    │   ├── AppTheme.cs                
    │   ├── UiHelpers.cs               
    │   └── BrandImages.cs             
    ├── Images/
    │   └── SolPlaatjeLogo.png         Kimberley municipality logo
    ├── Models/
    │   ├── IssueReport.cs             
    │   ├── EventItem.cs               
    │   ├── Announcement.cs            
    │   ├── SearchCriteria.cs          
    │   └── Recommendation.cs          
    ├── Services/
    │   ├── IssueRepository.cs       
    │   ├── EventRepository.cs         
    │   ├── UserActivityTracker.cs     
    │   └── RecommendationEngine.cs    
    └── Forms/
        ├── MainMenuForm.cs / .Designer.cs      
        ├── ReportIssueForm.cs / .Designer.cs   
        └── EventsForm.cs / .Designer.cs        
```

\---

## 3\. How to compile \& run

### Option A — Visual Studio

1. Open `MunicipalReportingApp.sln` in Visual Studio 2022.
2. Press **F5** (or **Ctrl+F5** to run without debugging).

### Option B — Command line

Open PowerShell or Command Prompt **in the folder that contains
`MunicipalReportingApp.sln`** (type `cmd` in File Explorer's address bar while
inside that folder), then run:

```bash
dotnet build
dotnet run --project MunicipalReportingApp
```

The first command compiles the solution and reports any errors; the second
starts the application.

To produce a standalone executable:

```bash
dotnet publish MunicipalReportingApp -c Release -r win-x64 --self-contained true
```

The compiled app will be under
`MunicipalReportingApp/bin/Release/net8.0-windows/win-x64/publish/`.

\---

## 4\. How to use our application

1. **Main Menu** — On launch you'll see three options:

   * **Report Issues** (active, Part 1)
   * **Local Events and Announcements** (active, Part 2 )
   * **Service Request Status** ( "coming soon")

   A footer message shows how many issues have been reported by the
community so far, encouraging participation.

2. **Report Issues screen** — Click *Report Issues* to open the reporting
form:

   * **Location** — type where the issue is (street name, landmark, etc.).
   * **Category** — pick from a dropdown (Sanitation \& Waste, Roads \&
Potholes, Water \& Utilities, Street Lighting, Parks \& Recreation,
Public Safety, Noise Complaint, Other).
   * **Description** — describe the issue in the rich text box.
   * **Attachments** — click *Attach Image / Document...* to open a file
picker (`OpenFileDialog`) and attach one or more photos or documents.
Attached file names appear in the list below the button.
   * **Engagement bar** — a progress bar and short message fill in live as
you complete each field, encouraging you to finish the report.
   * **Submit Report** — validates that Location, Category and Description
are filled in. If something is missing, a warning message box tells
you exactly what's needed. On success, you get a confirmation dialog
with a reference number, and the form resets so you can submit
another report if needed.
   * **Back to Main Menu** — returns to the start screen at any time.
3. **Data storage** — All submitted reports are kept in memory for the
session (`List<IssueReport>` inside `IssueRepository`) and also written
to a small `issues.json` file next to the executable, so reports survive
an application restart during testing/demo.

\---

## 5\. Part 2 — Local Events and Announcements: how to use it

Click **Local Events and Announcements** on the main menu. The window opens
maximised; everything resizes with it.

1. **Announcements strip (top)** — coloured cards, most urgent first
   (red = urgent, amber = important, blue = general). Click a card to read the
   full message.
2. **Search bar** — combine any of:
   * **Search events** — type one or more words (e.g. `music`, `clinic`). Every
     word must appear in the event's title, category, venue or description.
   * **Category** — choose one category, or *All categories*.
   * **From date / To date** — tick the box beside a date to use it. Past
     dates are not offered, because only upcoming events are shown.

   Press **Search** (or the Enter key). **Clear** resets all filters.
   **Previous search** steps back to the search you made before this one.
3. **Event list (left)** — upcoming events, grouped under a heading for each
   day and shown in date order. Each card shows the date, title, category,
   time and venue. Click a card to select it.
4. **Event details (right, top)** — the full description, date, time and venue
   of the selected event, plus a line of related events.
5. **Recommended for you (right, middle)** — suggestions learned from your
   searches and clicks, each with a short reason, e.g. *"Because you like
   Culture & Arts"*. Click a suggestion to open it. **Reset** forgets
   what the app has learned.
6. **Recently viewed / Recent searches (right, bottom)** — double-click an
   entry to open that event again or re-run that search.

Test this to see the recommendations change: search for `music`, open two
*Culture & Arts* events, then look at the **Recommended for you** panel.


> **Sample data:** the events and announcements are built in
> (`Services/EventRepository.cs`, method `SeedSampleData`). Their dates are
> set relative to today's date, so there are always upcoming events.
> The people, venues and details are examples only, not real municipal listings.

The app also writes `user_profile.json` next to the executable. It stores the
interest scores used for recommendations, so suggestions are still personal
after a restart. Delete the file (or press **Reset**) to start fresh.

\---

## 6\. Part 2 — data structures and algorithms

| Requirement | Structure | Where | What it is used for |
|---|---|---|---|
| Stacks | `Stack<SearchCriteria>` | `UserActivityTracker` | Every search made, newest on top. **Previous search** pops the top and shows the one below (last in, first out). |
| Stacks | `Stack<EventItem>` | `UserActivityTracker` | *Recently viewed* events, newest on top. |
| Queues | `Queue<SearchCriteria>` | `UserActivityTracker` | The last five searches. New ones join the back; the oldest leaves from the front (first in, first out). |
| Priority queues | `PriorityQueue<Announcement, …>` | `EventRepository.GetAnnouncementsByPriority` | Announcements come out urgent first, newest first within a level. |
| Priority queues | `PriorityQueue<Recommendation, …>` | `RecommendationEngine.GetRecommendations` | Keeps only the best *N* suggestions while scanning, without sorting every event. |
| Sorted dictionaries | `SortedDictionary<DateTime, List<EventItem>>` | `EventRepository` | Events grouped by day and kept in date order automatically. |
| Dictionaries | `Dictionary<int, EventItem>` | `EventRepository` | Look up an event by its Id. |
| Dictionaries | `Dictionary<string, int>` (two) | `RecommendationEngine` | The user's interest profile: category scores and keyword counts. |
| Hash tables | `Hashtable` (category → `List<int>`) | `EventRepository` | Category index: jump straight to the events of one category. |
| Sets | `HashSet<string>` | `EventRepository` | The unique categories (no duplicates). |
| Sets | `SortedSet<DateTime>` | `EventRepository` | The unique, ordered dates that have events. `GetViewBetween` gives an efficient date-range search. |
| Sets | `HashSet<int>` + `IntersectWith` | `EventRepository.Search` | Combines the category filter with the date filter by set intersection. |
| Sets | `HashSet<string>` | `RecommendationEngine` | Common words ignored when learning from keywords. |

### How a search works (`EventRepository.Search`)

1. `SortedSet.GetViewBetween` selects the dates inside the chosen range
   (today onwards, so past events never appear).
2. The ids of the events on those dates go into a `HashSet<int>`.
3. If a category was chosen, the `Hashtable` index supplies that category's ids
   and `IntersectWith` keeps only the events that match both.
4. A keyword filter is applied to what is left, and results are returned in
   date order.

### How the recommendation features workss (`RecommendationEngine`)

It is *frequency-weighted content-based filtering*: it suggests events that are
similar to the ones the user has shown interest in.

1. **Learning from behaviour**
   * Choosing a category in a search: **+2** to that category.
   * Opening an event: **+3** to its category, and **+1** to each meaningful
     word in its title.
   * Searching for a word: **+1** to that word, and **+1** to the categories
     of the events it found.
2. **Scoring every upcoming event** =
   its category's score
   + 2 × the count of each remembered word that appears in the event
   + 2 if it is within 7 days (or +1 within 14 days) — only for events that
   already match something.
3. **Choosing the best four** with a `PriorityQueue` (see table). Events already
   opened this visit are skipped so the panel keeps offering new ideas.
4. If there is not enough history, the soonest events fill the remaining places
   and are labelled *Coming up soon*.


\---


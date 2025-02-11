using CommonLayer.Models;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interface
{
    public interface INotesManager
    {
        public Notes CreateNote(int UserId, NotesModel notesModel);
        public List<Notes> GetAllNotes(int UserId);

        public bool UpdateNote(int NotesId, int UserId, UpdateNoteModel updateNoteModel);

        public bool DeleteNote(int NotesId, int UserId);
    }
}

import { Component, inject } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatIconModule } from "@angular/material/icon";

@Component({
    selector: 'app-confirm-delete-dialog',
    standalone: true,
    // 1. Wichtig: Fehlende Module für das Styling importieren
    imports: [MatDialogModule, MatButtonModule, MatIconModule],
    template: `
    <!-- Titel mit einem passenden Warn-Icon versehen -->
    <h2 mat-dialog-title class="delete-title">
      <mat-icon>warning</mat-icon>
      Delete Employee?
    </h2>

    <!-- Offizieller Inhaltsbereich für saubere Abstände -->
    <mat-dialog-content>
      <p>This action cannot be undone. All data associated with this employee will be permanently removed.</p>
    </mat-dialog-content>

    <!-- Strukturierte Aktionsknöpfe am unteren Rand -->
    <mat-dialog-actions align="end">
      <!-- Abbrechen bleibt dezent -->
      <button mat-button (click)="cancel()">
        Cancel
      </button>
      
      <!-- Löschen sticht im M3-Warn-Look (Rot) hervor -->
      <button mat-flat-button class="btn-danger" (click)="confirm()">
        Delete
      </button>
    </mat-dialog-actions>
  `,
    // 2. Inline-Styles für die M3-Warnfarben und Abstände
    styles: `
    .delete-title {
      display: flex;
      align-items: center;
      gap: 12px;
      color: var(--mat-sys-error);
    }

    .delete-title mat-icon {
      color: var(--mat-sys-error);
      --mdc-icon-size: 28px; /* Macht das Warn-Icon etwas markanter */
    }

    mat-dialog-content p {
      margin: 0;
      color: var(--mat-sys-on-surface-variant);
      font: var(--mat-sys-body-medium);
      line-height: 1.5;
    }

    /* Färbt den M3 Flat-Button in das Rot des Themes um */
    .btn-danger {
      background-color: var(--mat-sys-error) !important;
      color: var(--mat-sys-on-error) !important;
    }
  `
})
export class ConfirmDeleteDialog {
    private dialogRef = inject(MatDialogRef<ConfirmDeleteDialog>);

    confirm() {
        this.dialogRef.close(true);
    }

    cancel() {
        this.dialogRef.close(false);
    }
}

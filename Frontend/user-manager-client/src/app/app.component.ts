import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { firstValueFrom } from 'rxjs';
import { UsersService } from './users-api/services';
import { UserDto } from './users-api/models';
import { UserModalComponent } from './user/user-modal/user-modal.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'actions'];
  dataSource = new MatTableDataSource<UserDto>();
  editedUser: UserDto | null = null;
  users: UserDto[] = [];
  isLoading = false;

  constructor(private usersService: UsersService, public dialog: MatDialog) {}

  async ngOnInit() {
    await this.loadUsers();
  }

  async loadUsers() {
    this.isLoading = true;
    try {
      this.users = await firstValueFrom(this.usersService.getUsers());
    } finally {
      this.isLoading = false;
    }
  }

  startEdit(user: UserDto) {
    this.editedUser = { ...user };
  }

  saveEdit() {
    if (this.editedUser && this.editedUser.id) {
      this.usersService
        .putUser({
          id: this.editedUser.id,
          body: this.editedUser,
        })
        .subscribe(() => {
          this.loadUsers();
          this.editedUser = null;
        });
    }
  }

  openEditModal(user: UserDto): void {
    const dialogRef = this.dialog.open(UserModalComponent, {
      width: '250px',
      data: { ...user },
    });

    dialogRef.afterClosed().subscribe((result: UserDto) => {
      if (result && result.id) {
        this.usersService
          .putUser({
            id: result.id,
            body: result,
          })
          .subscribe(() => {
            this.loadUsers();
          });
      }
    });
  }

  deleteUser(id: number) {
    this.usersService.deleteUser({ id }).subscribe(() => {
      this.loadUsers();
    });
  }
}

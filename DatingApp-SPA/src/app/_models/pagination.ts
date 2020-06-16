// interface koj ke gi dobie vrednostite sa UsersParams // 143
export interface Pagination
{
currentPage: number;
itemsPerPage: number;
totalItems: number;
totalPages: number;
}

export class PaginatedResult<T> {
    result: T;
    pagination: Pagination;
}




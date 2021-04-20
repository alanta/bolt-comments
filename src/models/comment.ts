export interface Comment{
    id: string;
    key: string;
    name: string;
    email: string;
    content: string;
    posted: Date;
    approved: boolean;
}

export interface CommentsByKey{
    key: string;
    comments: Comment[]
}